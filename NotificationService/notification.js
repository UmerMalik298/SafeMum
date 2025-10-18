import express from "express";
import admin from "firebase-admin";
import fs from "fs";

const router = express.Router();

// --- Load Firebase Admin from a local file ONLY (no env vars) ---
const CRED_PATH = "firebase-service-account.json";
if (!fs.existsSync(CRED_PATH)) {
  throw new Error(
    `Missing ${CRED_PATH}. Please keep this file in NotificationService/ and commit it for now (not recommended for prod).`
  );
}
const serviceAccount = JSON.parse(fs.readFileSync(CRED_PATH, "utf8"));

if (!admin.apps.length) {
  admin.initializeApp({
    credential: admin.credential.cert(serviceAccount),
  });
  console.log("Firebase Admin initialized from local file.");
}

/**
 * POST /api/send-push
 * body:
 *  - { deviceToken, title, body, data? }
 *  - { deviceTokens: [], title, body, data? }
 */
router.post("/send-push", async (req, res) => {
  try {
    const { deviceToken, deviceTokens, title, body, data } = req.body;
    if (!title || !body || (!deviceToken && !deviceTokens?.length)) {
      return res.status(400).json({ success: false, error: "Missing fields" });
    }

    const tokens = deviceTokens?.length ? deviceTokens : [deviceToken];

    const message = {
      tokens,
      notification: { title, body },
      data: data
        ? Object.fromEntries(Object.entries(data).map(([k, v]) => [k, String(v)]))
        : {},
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

    const invalidTokens = [];
    response.responses.forEach((r, idx) => {
      if (!r.success) {
        const msg = r.error?.errorInfo?.message || r.error?.message || "";
        if (msg.includes("UNREGISTERED") || msg.includes("NOT_FOUND")) {
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
