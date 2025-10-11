// notification.js
import express from "express";
import admin from "firebase-admin";

const router = express.Router();

// Load service account from base64 env var or fallback to file for local dev
let serviceAccount;
if (process.env.FIREBASE_SERVICE_ACCOUNT_BASE64) {
  const json = Buffer.from(process.env.FIREBASE_SERVICE_ACCOUNT_BASE64, "base64").toString("utf8");
  serviceAccount = JSON.parse(json);
} else {
  // local developer fallback (ONLY if you have the file locally for dev)
  // Remove or ignore this file in production (add to .gitignore)
  import fs from "fs";
  serviceAccount = JSON.parse(fs.readFileSync("firebase-service-account.json", "utf8"));
}

// Initialize Firebase Admin if not initialized already
if (!admin.apps.length) {
  admin.initializeApp({
    credential: admin.credential.cert(serviceAccount),
  });
}

// POST /api/send-push
router.post("/send-push", async (req, res) => {
  try {
    const { deviceToken, title, body, data } = req.body;

    if (!deviceToken || !title || !body) {
      return res.status(400).json({ success: false, error: "Missing fields" });
    }

    const message = {
      token: deviceToken,
      notification: { title, body },
      data: data ? data : {},
      android: {
        priority: "high",
        notification: { sound: "default", channel_id: "appointments" },
      },
      apns: {
        payload: { aps: { alert: { title, body }, sound: "default", badge: 1 } },
      },
    };

    const response = await admin.messaging().send(message);
    res.json({ success: true, messageId: response });
  } catch (error) {
    console.error("Error sending notification:", error);
    res.status(500).json({ success: false, error: error.message });
  }
});

export default router;
