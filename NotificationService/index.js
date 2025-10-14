import express from "express";
import notificationRoutes from "./notification.js";
import dotenv from "dotenv";

dotenv.config();

const app = express();
app.use(express.json());

// 🔓 auth OFF by default. flip ON later by setting REQUIRE_API_KEY=true
if (process.env.REQUIRE_API_KEY === "true") {
  app.use((req, res, next) => {
    const key = req.header("X-API-KEY");
    if (!process.env.NOTIFY_API_KEY || key !== process.env.NOTIFY_API_KEY) {
      return res.status(401).json({ success: false, error: "Unauthorized" });
    }
    next();
  });
}

// Health check
app.get("/healthz", (_req, res) => res.status(200).send("ok"));

app.use("/api", notificationRoutes);

const port = process.env.PORT || 5001;
app.listen(port, () => {
  console.log(
    `Notification service running on port ${port} (auth ${
      process.env.REQUIRE_API_KEY === "true" ? "ON" : "OFF"
    })`
  );
});
