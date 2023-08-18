//Drop area

let file;
let validExtensions = ["application/excel", "application/vnd.ms-excel", "application/x-excel", "application/x-msexcel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"]; //adding some valid image extensions in array
  

const dropArea1 = document.querySelector(".first");
let dragText1 = dropArea1.querySelector("header");
let button1 = dropArea1.querySelector("button");
let input1 = dropArea1.querySelector("input");

const dropArea2 = document.querySelector(".second");
let dragText2 = dropArea2.querySelector("header");
let button2 = dropArea2.querySelector("button");
let input2 = dropArea2.querySelector("input");



//first DropArea

button1.onclick = ()=>{
  input1.click();
}
//If user Drop File via button
input1.addEventListener("change", function(){
  file = this.files[0];
  let fileType = file.type; //getting selected file type
  if(validExtensions.includes(fileType)){
    dropArea1.classList.add("active");
    //Create datatransfer to assign file to input
    var dT = new DataTransfer();
    dT.items.add(file);
    input1.files = dT.files;
    dragText1.textContent = "Файл "+input1.files[0].name+" загружен";
    button1.textContent = "Выбрать другой файл";
  }else{
    alert("Тип файла не поддерживается!");
    dropArea1.classList.remove("active");
    if(input1.files[0] == undefined)
      dragText1.textContent = "Перетащите файл";
  }
});
//If user Drop File Over DropArea
dropArea1.addEventListener("dragover", (event)=>{
  event.preventDefault(); //preventing from default behaviour
  dropArea1.classList.add("active");
  if(input1.files[0] == undefined)
    {dragText1.textContent = "Отпустите для загрузки";
    button1.textContent = "Выбрать файл";}
});
//If user leave DropArea while holding file
dropArea1.addEventListener("dragleave", ()=>{
  dropArea1.classList.remove("active");
  if(input1.files[0] == undefined)
    {dragText1.textContent = "Перетащите файл";
    button1.textContent = "Выбрать файл";}
});
//If user drop File on DropArea
dropArea1.addEventListener("drop", (event)=>{
  event.preventDefault(); //preventing from default behaviour
  file = event.dataTransfer.files[0];
  let fileType = file.type; //getting selected file type
  if(validExtensions.includes(fileType)){
    dropArea1.classList.add("active");
    //Create datatransfer to assign file to input
    var dT = new DataTransfer();
    dT.items.add(file);
    input1.files = dT.files;
    dragText1.textContent = "Файл "+input1.files[0].name+" загружен";
    button1.textContent = "Выбрать другой файл";
  }else{
    alert("Тип файла не поддерживается!");
    dropArea1.classList.remove("active");
    if(input1.files[0] == undefined)
      dragText1.textContent = "Перетащите файл";
  }
});



//second DropArea

button2.onclick = ()=>{
  input2.click();
}
//If user Drop File via button
input2.addEventListener("change", function(){
  file = this.files[0];
  let fileType = file.type; //getting selected file type
  if(validExtensions.includes(fileType)){
    dropArea2.classList.add("active");
    //Create datatransfer to assign file to input
    var dT2 = new DataTransfer();
    dT2.items.add(file);
    input2.files = dT2.files;
    dragText2.textContent = "Файл "+input2.files[0].name+" загружен";
    button2.textContent = "Выбрать другой файл";
  }else{
    alert("Тип файла не поддерживается!");
    dropArea2.classList.remove("active");
    if(input2.files[0] == undefined)
      dragText2.textContent = "Перетащите файл";
  }
});
//If user Drop File Over DropArea
dropArea2.addEventListener("dragover", (event)=>{
  event.preventDefault(); //preventing from default behaviour
  dropArea2.classList.add("active");
  if(input2.files[0] == undefined)
    {dragText2.textContent = "Отпустите для загрузки";
    button2.textContent = "Выбрать файл";}
});
//If user leave DropArea while holding file
dropArea2.addEventListener("dragleave", ()=>{
  dropArea2.classList.remove("active");
  if(input2.files[0] == undefined)
    {dragText2.textContent = "Перетащите файл";
    button2.textContent = "Выбрать файл";}
});
//If user drop File on DropArea
dropArea2.addEventListener("drop", (event)=>{
  event.preventDefault(); //preventing from default behaviour
  file = event.dataTransfer.files[0];
  let fileType = file.type; //getting selected file type
  if(validExtensions.includes(fileType)){
    dropArea2.classList.add("active");
    //Create datatransfer to assign file to input
    var dT2 = new DataTransfer();
    dT2.items.add(file);
    input2.files = dT2.files;
    dragText2.textContent = "Файл "+input2.files[0].name+" загружен";
    button2.textContent = "Выбрать другой файл";
  }else{
    alert("Тип файла не поддерживается!");
    dropArea2.classList.remove("active");
    if(input2.files[0] == undefined)
      dragText2.textContent = "Перетащите файл";
  }
});