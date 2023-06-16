const connection = new signalR.HubConnectionBuilder()
    .withUrl("/connection") // Replace with the actual URL of your SignalR hub
    .build();

connection.start().then(() => { }).catch((error) => {
    console.error(error);
});

connection.on("OrderPlaced", (order) => {
    console.log(order);

    if (Notification.permission === "granted") {
        showNotification(order);
    } else if (Notification.permission !== "denied") {
        Notification.requestPermission().then((permission) => {
            if (permission === "granted") {
                showNotification(order);
            }
        });
    }
});

function showNotification(order) {
    const title = "New Order Placed";
    const options = {
        body: JSON.stringify(order),
    };

    // Create a new notification
    const notification = new Notification(title, options);

    // Optional: Handle click event on the notification
    notification.onclick = () => {
        // Do something when the user clicks the notification
    };
}

//For Searching Category
$('.selectpicker').selectpicker();