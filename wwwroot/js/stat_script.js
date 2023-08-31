document.addEventListener("DOMContentLoaded", function () {
    tabContent = document.getElementsByClassName("tab-content");
    tabContent[0].style.display = "block";
});

let selectedblock = null;

function openTab(evt, tabName) {
    var i, tabContent;
    tabContent = document.getElementsByClassName("tab-content");
    for (i = 0; i < tabContent.length; i++) {
        tabContent[i].style.display = "none";
    }
    document.getElementById(tabName).style.display = "block";
}

function editProf(button) {
    var index = button.parentNode.querySelector(".index").value;
    var a = document.createElement('a');
    a.href = "#profpopup";
    a.click();
    var popup = document.querySelector("#profpopup");
    var tab = document.querySelector("#tab1");
    var tabBody = tab.querySelectorAll("tbody");
    popup.querySelector("#FIO").value = tabBody[index].querySelector("#FIO").innerHTML;
    popup.querySelector("#cnumber").value = tabBody[index].querySelector("#cnumber").innerHTML;
    popup.querySelector("#gnumber").value = tabBody[index].querySelector("#gnumber").innerHTML;
    popup.querySelector("#theme").value = tabBody[index].querySelector("#theme").innerHTML;
    popup.querySelector("#tel").value = tabBody[index].querySelector("#tel").innerHTML;
    popup.querySelector("#mail").value = tabBody[index].querySelector("#email").innerHTML;
    popup.querySelector("#id").value = tabBody[index].querySelector("#id").innerHTML;

    selectedblock = tabBody[index];

    const scientistSelect = popup.querySelector(".popup_input[name='scientist']");
    const scientistText = tabBody[index].querySelector("#scientist").textContent;
    const options = Array.from(scientistSelect.options);
    const optionToUpdate = options[0];
    optionToUpdate.selected = true;
    const optionToSelect = options.find(item => item.value == scientistText);
    if (optionToSelect != undefined)
        optionToSelect.selected = true;

    const dirSelect = popup.querySelector(".popup_input[name='stDir']");
    const dirText = tabBody[index].querySelector("#stDir").textContent;
    const diroptions = Array.from(dirSelect.options);
    const diroptionToUpdate = diroptions[0];
    diroptionToUpdate.selected = true;
    const diroptionToSelect = diroptions.find(item => item.value === dirText);
    if (diroptionToSelect != undefined) {
        diroptionToSelect.selected = true;
    }
    else {
        const profSelect = popup.querySelector(".popup_input[name='stProf']");
        profSelect.innerHTML = '<option value="NULL">Сначала выберите направление подготовки</option>';
    }

    const event = new Event("change");
    dirSelect.dispatchEvent(event);
}

function editAdm(button) {
    var index = button.parentNode.querySelector(".index").value;
    var a = document.createElement('a');
    a.href = "#adminpopup";
    a.click();
    var popup = document.querySelector("#adminpopup");
    var tab = document.querySelector("#tab2");
    var tabBody = tab.querySelectorAll("tbody");
    popup.querySelector("#FIO").value = tabBody[index].querySelector("#FIO").innerHTML;
    popup.querySelector("#function").value = tabBody[index].querySelector("#function").innerHTML;
    popup.querySelector("#cab").value = tabBody[index].querySelector("#cab").innerHTML;
    popup.querySelector("#roleLvl").value = tabBody[index].querySelector("#roleLvl").innerHTML;
    popup.querySelector("#tel").value = tabBody[index].querySelector("#tel").innerHTML;
    popup.querySelector("#mail").value = tabBody[index].querySelector("#mail").innerHTML;
    popup.querySelector("#id").value = tabBody[index].querySelector("#id").innerHTML;
}

function editStip(button) {
    var index = button.parentNode.querySelector(".index").value;
    var a = document.createElement('a');
    a.href = "#stippopup";
    a.click();
    var popup = document.querySelector("#stippopup");
    var tab = document.querySelector("#tab3");
    var tabBody = tab.querySelectorAll("tbody");
    popup.querySelector("#name").value = tabBody[index].querySelector("#name").innerHTML;
    popup.querySelector("#amount").value = tabBody[index].querySelector("#amount").innerHTML;
    popup.querySelector("#startDate").value = tabBody[index].querySelector("#startDate").innerHTML;
    popup.querySelector("#endDate").value = tabBody[index].querySelector("#endDate").innerHTML;
    popup.querySelector("#aspId").value = tabBody[index].querySelector("#aspId").innerHTML;
    popup.querySelector("#id").value = tabBody[index].querySelector("#id").innerHTML;
}

function editRes(button) {
    var index = button.parentNode.querySelector(".index").value;
    var a = document.createElement('a');
    a.href = "#respopup";
    a.click();
    var popup = document.querySelector("#respopup");
    var tab = document.querySelector("#tab4");
    var tabBody = tab.querySelectorAll("tbody");
    popup.querySelector("#subj").value = tabBody[index].querySelector("#subj").innerHTML;
    popup.querySelector("#result").value = tabBody[index].querySelector("#result").innerHTML;
    popup.querySelector("#type").value = tabBody[index].querySelector("#type").innerHTML;
    popup.querySelector("#date").value = tabBody[index].querySelector("#date").innerHTML;
    popup.querySelector("#aspId").value = tabBody[index].querySelector("#aspId").innerHTML;
    popup.querySelector("#id").value = tabBody[index].querySelector("#id").innerHTML;
}

function editTimet(button) {
    var index = button.parentNode.querySelector(".index").value;
    var a = document.createElement('a');
    a.href = "#timetpopup";
    a.click();
    var popup = document.querySelector("#timetpopup");
    var tab = document.querySelector("#tab5");
    var tabBody = tab.querySelectorAll("tbody");
    popup.querySelector("#name").value = tabBody[index].querySelector("#name").innerHTML;
    popup.querySelector("#date").value = tabBody[index].querySelector("#date").innerHTML;
    popup.querySelector("#type").value = tabBody[index].querySelector("#type").innerHTML;
    popup.querySelector("#id").value = tabBody[index].querySelector("#id").innerHTML;
}

function editAchiev(button) {
    var index = button.parentNode.querySelector(".index").value;
    var a = document.createElement('a');
    a.href = "#achievpopup";
    a.click();
    var popup = document.querySelector("#achievpopup");
    var tab = document.querySelector("#tab6");
    var tabBody = tab.querySelectorAll("tbody");
    popup.querySelector("#name").value = tabBody[index].querySelector("#name").innerHTML;
    popup.querySelector("#date").value = tabBody[index].querySelector("#date").innerHTML;
    popup.querySelector("#aspId").value = tabBody[index].querySelector("#aspId").innerHTML;
    popup.querySelector("#id").value = tabBody[index].querySelector("#id").innerHTML;
}

function editDoc(button) {
    var index = button.parentNode.querySelector(".index").value;
    var a = document.createElement('a');
    a.href = "#docpopup";
    a.click();
    var popup = document.querySelector("#docpopup");
    var tab = document.querySelector("#tab7");
    var tabBody = tab.querySelectorAll("tbody");
    popup.querySelector("#name").value = tabBody[index].querySelector("#name").innerHTML;
    popup.querySelector("#date").value = tabBody[index].querySelector("#date").innerHTML;
    popup.querySelector("#aspId").value = tabBody[index].querySelector("#aspId").innerHTML;
    popup.querySelector("#id").value = tabBody[index].querySelector("#id").innerHTML;
}

function editScience(button) {
    var index = button.parentNode.querySelector(".index").value;
    var a = document.createElement('a');
    a.href = "#sciencepopup";
    a.click();
    var popup = document.querySelector("#sciencepopup");
    var tab = document.querySelector("#tab8");
    var tabBody = tab.querySelectorAll("tbody");
    popup.querySelector("#FIO").value = tabBody[index].querySelector("#FIO").innerHTML;
    popup.querySelector("#degree").value = tabBody[index].querySelector("#degree").innerHTML;
    popup.querySelector("#depart").value = tabBody[index].querySelector("#depart").innerHTML;
    popup.querySelector("#tel").value = tabBody[index].querySelector("#tel").innerHTML;
    popup.querySelector("#mail").value = tabBody[index].querySelector("#mail").innerHTML;
    popup.querySelector("#id").value = tabBody[index].querySelector("#id").innerHTML;
}

function editDirection(button) {
    var index = button.parentNode.querySelector(".index").value;
    var a = document.createElement('a');
    a.href = "#directpopup";
    a.click();
    var popup = document.querySelector("#directpopup");
    var tab = document.querySelector("#tab9");
    var tabBody = tab.querySelectorAll("tbody");
    popup.querySelector("#depart").value = tabBody[index].querySelector("#depart").innerHTML;
    popup.querySelector("#prof").value = tabBody[index].querySelector("#prof").innerHTML;
    popup.querySelector("#name").value = tabBody[index].querySelector("#name").innerHTML;
    popup.querySelector("#id").value = tabBody[index].querySelector("#id").innerHTML;
}

function editNew(button) {
    var index = button.parentNode.querySelector(".index").value;
    var a = document.createElement('a');
    a.href = "#newspopup";
    a.click();
    var popup = document.querySelector("#newspopup");
    var tab = document.querySelector("#tab10");
    var tabBody = tab.querySelectorAll("tbody");
    popup.querySelector("#info").value = tabBody[index].querySelector("#info").innerHTML;
    popup.querySelector("#link").value = tabBody[index].querySelector("#link").innerHTML;
    popup.querySelector("#date").value = tabBody[index].querySelector("#date").innerHTML;
    popup.querySelector("#id").value = tabBody[index].querySelector("#id").innerHTML;
}

function editPM(button) {
    var index = button.parentNode.querySelector(".index").value;
    var a = document.createElement('a');
    a.href = "#pmpopup";
    a.click();
    var popup = document.querySelector("#pmpopup");
    var tab = document.querySelector("#tab11");
    var tabBody = tab.querySelectorAll("tbody");
    popup.querySelector("#file").value = tabBody[index].querySelector("#file").innerHTML;
    popup.querySelector("#date").value = tabBody[index].querySelector("#date").innerHTML;
    popup.querySelector("#aspid").value = tabBody[index].querySelector("#aspid").innerHTML;
    popup.querySelector("#id").value = tabBody[index].querySelector("#id").innerHTML;
}


const hidden_div = document.querySelector(".hidden_div");
const profpopup = document.getElementById("profpopup");
const directionSelector = profpopup.querySelector(".popup_input[name='stDir']");

directionSelector.addEventListener("change", function () {

    const profText = selectedblock.querySelector("#stProf").textContent;
    const profSelect = profpopup.querySelector(".popup_input[name='stProf']");

    const dirText = directionSelector.value;
    const dirBlocks = document.querySelectorAll(".dir-block");
    profSelect.innerHTML = '<option value="NULL">Выберите профиль подготовки</option>';
    dirBlocks.forEach(block => {
        const dirName = block.querySelector("#name");
        if (dirName.textContent === dirText) {
            const profName = block.querySelector("#profile");
            const option = document.createElement("option");
            option.value = profName.textContent;
            option.text = profName.textContent;
            profSelect.appendChild(option);
        }
    });

    const profoptions = Array.from(profSelect.options);
    const profoptionToSelect = profoptions.find(item => item.value === profText);
    if (profoptionToSelect != undefined) {
        profoptionToSelect.selected = true;
    }
});