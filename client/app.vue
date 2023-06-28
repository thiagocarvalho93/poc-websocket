<template>
  <div>
    <NuxtWelcome />
  </div>
</template>
<script>
export default {
  mounted() {
    window.Notification.requestPermission((permission) => {
      if (permission == "granted") {
        this.sendNotification();
      }
    });
    this.initializeWebSocket();
  },
  methods: {
    sendNotification(message) {
      if (!("Notification" in window)) {
        // Check if the browser supports notifications
        alert("This browser does not support desktop notification");
      } else if (Notification.permission === "granted") {
        // Check whether notification permissions have already been granted;
        // if so, create a notification
        const notification = new Notification(message);
        // …
      } else if (Notification.permission !== "denied") {
        // We need to ask the user for permission
        Notification.requestPermission().then((permission) => {
          // If the user accepts, let's create a notification
          if (permission === "granted") {
            const notification = new Notification(message);
            // …
          }
        });
      }
    },

    initializeWebSocket() {
      const socket = new WebSocket("ws://localhost:5147");

      socket.onopen = () => {
        console.log("WebSocket connection established.");
      };

      socket.onmessage = (event) => {
        console.log("Message received:", event.data);
        this.sendNotification(event);
      };

      socket.onclose = () => {
        console.log("WebSocket connection closed.");
        // tentar reconectar
      };

      socket.onerror = (error) => {
        console.error("WebSocket error:", error);
      };
    },
  },
};
</script>
