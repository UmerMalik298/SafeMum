import express from "express";
import notificationRoutes from "./notification.js";

const app = express();
app.use(express.json());

// 🔐 Auth OFF (no API key) since you asked to avoid env vars for now
// If you later want auth, add a simple header check here.

app.get("/healthz", (_req, res) => res.status(200).send("ok"));
app.use("/api", notificationRoutes);

const port = process.env.PORT || 5001;
app.listen(port, () => {
  console.log(`Notification service running on port ${port}`);
});
