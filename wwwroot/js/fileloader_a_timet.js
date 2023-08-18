//Drop area

let file;
let validExtensions = ["application/pdf"]; //adding some valid image extensions in array


const dropArea1 = document.querySelector(".first");
let dragText1 = dropArea1.querySelector("header");
let button1 = dropArea1.querySelector("button");
let input1 = dropArea1.querySelector("input");

const dropArea2 = document.querySelector(".second");
let dragText2 = dropArea2.querySelector("header");
let button2 = dropArea2.querySelector("button");
let input2 = dropArea2.querySelector("input");

const dropArea3 = document.querySelector(".third");
let dragText3 = dropArea3.querySelector("header");
let button3 = dropArea3.querySelector("button");
let input3 = dropArea3.querySelector("input");

const dropArea4 = document.querySelector(".fourth");
let dragText4 = dropArea4.querySelector("header");
let button4 = dropArea4.querySelector("button");
let input4 = dropArea4.querySelector("input");



//first DropArea

button1.onclick = () => {
  input1.click();
}
//If user Drop File via button
input1.addEventListener("change", function () {
  var dT = new DataTransfer();
  for (let i = 0; i < this.files.length; i++) {
    file = this.files[i];
    let fileType = file.type; //getting selected file type
    if (validExtensions.includes(fileType)) {
      dropArea1.classList.add("active");
      //Create datatransfer to assign file to input
      dT.items.add(file);
    }
  }
  input1.files = dT.files;
  dragText1.textContent = input1.files.length + " файла(ов) загружено";
  button1.textContent = "Выбрать другой файл";
});
//If user Drop File Over DropArea
dropArea1.addEventListener("dragover", (event) => {
  event.preventDefault(); //preventing from default behaviour
  dropArea1.classList.add("active");
  if (input1.files[0] == undefined) {
    dragText1.textContent = "Отпустите для загрузки";
    button1.textContent = "Выбрать файл";
  }
});
//If user leave DropArea while holding file
dropArea1.addEventListener("dragleave", () => {
  dropArea1.classList.remove("active");
  if (input1.files[0] == undefined) {
    dragText1.textContent = "Перетащите файл";
    button1.textContent = "Выбрать файл";
  }
});
//If user drop File on DropArea
dropArea1.addEventListener("drop", (event) => {
  event.preventDefault(); //preventing from default behaviour
  var dT = new DataTransfer();
  for (let i = 0; i < event.dataTransfer.files.length; i++) {
    file = event.dataTransfer.files[i];
    let fileType = file.type; //getting selected file type
    if (validExtensions.includes(fileType)) {
      dropArea1.classList.add("active");
      //Create datatransfer to assign file to input
      dT.items.add(file);
    }
    input1.files = dT.files;
    dragText1.textContent = input1.files.length + " файла(ов) загружено";
    button1.textContent = "Выбрать другой файл";
  }
});




//second DropArea

button2.onclick = () => {
  input2.click();
}
//If user Drop File via button
input2.addEventListener("change", function () {
  var dT = new DataTransfer();
  for (let i = 0; i < this.files.length; i++) {
    file = this.files[i];
    let fileType = file.type; //getting selected file type
    if (validExtensions.includes(fileType)) {
      dropArea2.classList.add("active");
      //Create datatransfer to assign file to input
      dT.items.add(file);
    }
  }
  input2.files = dT.files;
  dragText2.textContent = input2.files.length + " файла(ов) загружено";
  button2.textContent = "Выбрать другой файл";
});
//If user Drop File Over DropArea
dropArea2.addEventListener("dragover", (event) => {
  event.preventDefault(); //preventing from default behaviour
  dropArea2.classList.add("active");
  if (input2.files[0] == undefined) {
    dragText2.textContent = "Отпустите для загрузки";
    button2.textContent = "Выбрать файл";
  }
});
//If user leave DropArea while holding file
dropArea2.addEventListener("dragleave", () => {
  dropArea2.classList.remove("active");
  if (input2.files[0] == undefined) {
    dragText2.textContent = "Перетащите файл";
    button2.textContent = "Выбрать файл";
  }
});
//If user drop File on DropArea
dropArea2.addEventListener("drop", (event) => {
  event.preventDefault(); //preventing from default behaviour
  var dT = new DataTransfer();
  for (let i = 0; i < event.dataTransfer.files.length; i++) {
    file = event.dataTransfer.files[i];
    let fileType = file.type; //getting selected file type
    if (validExtensions.includes(fileType)) {
      dropArea2.classList.add("active");
      //Create datatransfer to assign file to input
      dT.items.add(file);
    }
    input2.files = dT.files;
    dragText2.textContent = input2.files.length + " файла(ов) загружено";
    button2.textContent = "Выбрать другой файл";
  }
});



//third DropArea

button3.onclick = () => {
  input3.click();
}
//If user Drop File via button
input3.addEventListener("change", function () {
  var dT = new DataTransfer();
  for (let i = 0; i < this.files.length; i++) {
    file = this.files[i];
    let fileType = file.type; //getting selected file type
    if (validExtensions.includes(fileType)) {
      dropArea3.classList.add("active");
      //Create datatransfer to assign file to input
      dT.items.add(file);
    }
  }
  input3.files = dT.files;
  dragText3.textContent = input3.files.length + " файла(ов) загружено";
  button3.textContent = "Выбрать другой файл";
});
//If user Drop File Over DropArea
dropArea3.addEventListener("dragover", (event) => {
  event.preventDefault(); //preventing from default behaviour
  dropArea3.classList.add("active");
  if (input3.files[0] == undefined) {
    dragText3.textContent = "Отпустите для загрузки";
    button3.textContent = "Выбрать файл";
  }
});
//If user leave DropArea while holding file
dropArea3.addEventListener("dragleave", () => {
  dropArea3.classList.remove("active");
  if (input3.files[0] == undefined) {
    dragText3.textContent = "Перетащите файл";
    button3.textContent = "Выбрать файл";
  }
});
//If user drop File on DropArea
dropArea3.addEventListener("drop", (event) => {
  event.preventDefault(); //preventing from default behaviour
  var dT = new DataTransfer();
  for (let i = 0; i < event.dataTransfer.files.length; i++) {
    file = event.dataTransfer.files[i];
    let fileType = file.type; //getting selected file type
    if (validExtensions.includes(fileType)) {
      dropArea3.classList.add("active");
      //Create datatransfer to assign file to input
      dT.items.add(file);
    }
    input3.files = dT.files;
    dragText3.textContent = input3.files.length + " файла(ов) загружено";
    button3.textContent = "Выбрать другой файл";
  }
});



//fourth DropArea

button4.onclick = () => {
  input4.click();
}
//If user Drop File via button
input4.addEventListener("change", function () {
  var dT = new DataTransfer();
  for (let i = 0; i < this.files.length; i++) {
    file = this.files[i];
    let fileType = file.type; //getting selected file type
    if (validExtensions.includes(fileType)) {
      dropArea4.classList.add("active");
      //Create datatransfer to assign file to input
      dT.items.add(file);
    }
  }
  input4.files = dT.files;
  dragText4.textContent = input4.files.length + " файла(ов) загружено";
  button4.textContent = "Выбрать другой файл";
});
//If user Drop File Over DropArea
dropArea4.addEventListener("dragover", (event) => {
  event.preventDefault(); //preventing from default behaviour
  dropArea4.classList.add("active");
  if (input4.files[0] == undefined) {
    dragText4.textContent = "Отпустите для загрузки";
    button4.textContent = "Выбрать файл";
  }
});
//If user leave DropArea while holding file
dropArea4.addEventListener("dragleave", () => {
  dropArea4.classList.remove("active");
  if (input4.files[0] == undefined) {
    dragText4.textContent = "Перетащите файл";
    button4.textContent = "Выбрать файл";
  }
});
//If user drop File on DropArea
dropArea4.addEventListener("drop", (event) => {
  event.preventDefault(); //preventing from default behaviour
  var dT = new DataTransfer();
  for (let i = 0; i < event.dataTransfer.files.length; i++) {
    file = event.dataTransfer.files[i];
    let fileType = file.type; //getting selected file type
    if (validExtensions.includes(fileType)) {
      dropArea4.classList.add("active");
      //Create datatransfer to assign file to input
      dT.items.add(file);
    }
    input4.files = dT.files;
    dragText4.textContent = input4.files.length + " файла(ов) загружено";
    button4.textContent = "Выбрать другой файл";
  }
});