document.addEventListener("DOMContentLoaded", function () {
    const notification = document.getElementById("notification");
    const closeBtn = document.getElementById("close");
    const notificationValue = document.querySelector("#notificationValue");

    if (notificationValue.value == "1") {
        notification.style.backgroundColor = "#88cf88";
        showNotification("Действие выполнено успешно");
    }
    else if (notificationValue.value == "2") {
        notification.style.backgroundColor = "#f2948a";
        showNotification("Действие не выполнено. Произошла ошибка");
    }
    else if (notificationValue.value == "3") {
        notification.style.backgroundColor = "#88cf88";
        showNotification("Фото отправлено на модерацию");
    }
    else if (notificationValue.value == "4") {
        notification.style.backgroundColor = "#88cf88";
        showNotification("Сообщение отправлено");
    }

    closeBtn.addEventListener("click", function () {
        notification.style.display = "none";
    });
});

function showNotification(message) {
    notification.innerText = message;
    notification.style.display = "block";
    notification.style.animation = "fadeOut 1.5s 3s ease-in-out forwards";
    setTimeout(function () {
        notification.style.display = "none";
    }, 5000);
};