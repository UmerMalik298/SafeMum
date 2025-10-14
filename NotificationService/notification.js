import express from "express";
import admin from "firebase-admin";
import fs from "fs";

const router = express.Router();

// Load service account from base64 env var or local file (dev only)
let serviceAccount;
if (process.env.FIREBASE_SERVICE_ACCOUNT_BASE64) {
  const json = Buffer.from(
    process.env.FIREBASE_SERVICE_ACCOUNT_BASE64,
    "base64"
  ).toString("utf8");
  serviceAccount = JSON.parse(json);
} else if (fs.existsSync("firebase-service-account.json")) {
  // local developer fallback (do NOT bake this file into prod images)
  serviceAccount = JSON.parse(
    fs.readFileSync("firebase-service-account.json", "utf8")
  );
} else {
  throw new Error(
    "No Firebase credentials found. Set FIREBASE_SERVICE_ACCOUNT_BASE64 or provide firebase-service-account.json for local dev."
  );
}

// Initialize Firebase Admin once
if (!admin.apps.length) {
  admin.initializeApp({
    credential: admin.credential.cert(serviceAccount),
  });
}

/**
 * POST /api/send-push
 * Supports either:
 *  - { deviceToken, title, body, data? }
 *  - { deviceTokens: [], title, body, data? }
 */
router.post("/send-push", async (req, res) => {
  try {
    const { deviceToken, deviceTokens, title, body, data } = req.body;

    if (!title || !body || (!deviceToken && !deviceTokens?.length)) {
      return res
        .status(400)
        .json({ success: false, error: "Missing fields" });
    }

    const tokens = deviceTokens?.length ? deviceTokens : [deviceToken];

    const message = {
      tokens,
      notification: { title, body },
      data: data ? Object.fromEntries(Object.entries(data).map(([k, v]) => [k, String(v)])) : {},
      android: {
        priority: "high",
        notification: { sound: "default", channel_id: "appointments" }
      },
      apns: {
        headers: { "apns-priority": "10" },
        payload: { aps: { alert: { title, body }, sound: "default", badge: 1 } }
      }
    };

    const response = await admin.messaging().sendEachForMulticast(message);

    // Optional: collect invalid tokens for cleanup
    const invalidTokens = [];
    response.responses.forEach((r, idx) => {
      if (!r.success) {
        const err = r.error?.errorInfo?.message || r.error?.message || "";
        if (err.includes("UNREGISTERED") || err.includes("NOT_FOUND")) {
          invalidTokens.push(tokens[idx]);
        }
      }
    });

    res.json({
      success: true,
      sentCount: response.successCount,
      failCount: response.failureCount,
      invalidTokens
    });
  } catch (error) {
    console.error("Error sending notification:", error);
    res.status(500).json({ success: false, error: error.message });
  }
});

export default router;
