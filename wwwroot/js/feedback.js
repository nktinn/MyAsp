var button = document.querySelector("#feedback_btn");

button.addEventListener("click", function() {
    document.getElementById("feedback_form").style.display = "block";
    document.querySelector(".feedback_body").style.height = "30vh";
    button.style.display = "none";
});