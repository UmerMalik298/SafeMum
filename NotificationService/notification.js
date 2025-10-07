import express from "express";
import admin from "firebase-admin";
import fs from "fs";

const router = express.Router();

// Load service account key (from Firebase)
const serviceAccount = JSON.parse(
  fs.readFileSync("firebase-service-account.json", "utf8")
);

// Initialize Firebase Admin if not initialized already
if (!admin.apps.length) {
  admin.initializeApp({
    credential: admin.credential.cert(serviceAccount),
  });
}

// POST /send-push
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
