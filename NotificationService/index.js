// index.js
import express from "express";
import notificationRoutes from "./notification.js";

const app = express();
app.use(express.json()); // simpler than body-parser

app.use("/api", notificationRoutes);

const port = process.env.PORT || 5001;
app.listen(port, () => {
  console.log(`Notification service running on port ${port}`);
});
