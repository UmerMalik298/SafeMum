const express = require("express");
const bodyParser = require("body-parser");
const admin = require("firebase-admin");

// init firebase admin
const serviceAccount = require("./serviceAccountKey.json");

admin.initializeApp({
  credential: admin.credential.cert(serviceAccount),
});

const app = express();
app.use(bodyParser.json());

// route for push notification
app.post("/send-notification", async (req, res) => {
  try {
    const { deviceToken, title, body } = req.body;
    if (!deviceToken || !title || !body) {
      return res.status(400).json({ error: "Missing required fields" });
    }

    const message = {
      notification: { title, body },
      token: deviceToken,
    };

    const response = await admin.messaging().send(message);
    console.log("Notification sent:", response);
    res.json({ success: true, response });
  } catch (err) {
    console.error("Error sending notification:", err);
    res.status(500).json({ success: false, error: err.message });
  }
});

const PORT = 5001;
app.listen(PORT, () => console.log(`Notification service running on port ${PORT}`));
