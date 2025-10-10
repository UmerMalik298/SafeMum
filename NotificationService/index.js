import express from "express";
import bodyParser from "body-parser";
import notificationRoutes from './notification.js';


const app = express();
app.use(bodyParser.json());

app.use("/api", notificationRoutes);

app.listen(5001, () => {
  console.log("Notification service running on port 5001");
});
