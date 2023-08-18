document.addEventListener("DOMContentLoaded", function () {
    tabContent = document.getElementsByClassName("tab-content");
    tabContent[0].style.display = "block";
});

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
    console.log(index);
    a.click();
    var popup = document.querySelector("#profpopup");
    console.log(popup);
    var tab = document.querySelector("#tab1");
    console.log(tab);
    var tabBody = tab.querySelectorAll("tbody");
    console.log(tabBody);
    popup.querySelector("#FIO").value = tabBody[index].querySelector("#FIO").innerHTML;
    popup.querySelector("#cnumber").value = tabBody[index].querySelector("#cnumber").innerHTML;
    popup.querySelector("#gnumber").value = tabBody[index].querySelector("#gnumber").innerHTML;
    popup.querySelector("#stDir").value = tabBody[index].querySelector("#stDir").innerHTML;
    popup.querySelector("#stProf").value = tabBody[index].querySelector("#stProf").innerHTML;
    popup.querySelector("#theme").value = tabBody[index].querySelector("#theme").innerHTML;
    popup.querySelector("#scientist").value = tabBody[index].querySelector("#scientist").innerHTML;
    popup.querySelector("#tel").value = tabBody[index].querySelector("#tel").innerHTML;
    popup.querySelector("#mail").value = tabBody[index].querySelector("#email").innerHTML;
    popup.querySelector("#id").value = tabBody[index].querySelector("#id").innerHTML;
}

function editAdm(button) {
    var index = button.parentNode.querySelector(".index").value;
    var a = document.createElement('a');
    a.href = "#adminpopup";
    a.click();
    var popup = document.querySelector("#adminpopup");
    console.log(popup);
    var tab = document.querySelector("#tab2");
    console.log(tab);
    var tabBody = tab.querySelectorAll("tbody");
    console.log(tabBody);
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
    console.log(popup);
    var tab = document.querySelector("#tab3");
    console.log(tab);
    var tabBody = tab.querySelectorAll("tbody");
    console.log(tabBody);
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
    console.log(popup);
    var tab = document.querySelector("#tab4");
    console.log(tab);
    var tabBody = tab.querySelectorAll("tbody");
    console.log(tabBody);
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
    console.log(popup);
    var tab = document.querySelector("#tab5");
    console.log(tab);
    var tabBody = tab.querySelectorAll("tbody");
    console.log(tabBody);
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
    console.log(popup);
    var tab = document.querySelector("#tab6");
    console.log(tab);
    var tabBody = tab.querySelectorAll("tbody");
    console.log(tabBody);
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
    console.log(popup);
    var tab = document.querySelector("#tab7");
    console.log(tab);
    var tabBody = tab.querySelectorAll("tbody");
    console.log(tabBody);
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
    console.log(popup);
    var tab = document.querySelector("#tab8");
    console.log(tab);
    var tabBody = tab.querySelectorAll("tbody");
    console.log(tabBody);
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
    console.log(popup);
    var tab = document.querySelector("#tab9");
    console.log(tab);
    var tabBody = tab.querySelectorAll("tbody");
    console.log(tabBody);
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
    console.log(popup);
    var tab = document.querySelector("#tab10");
    console.log(tab);
    var tabBody = tab.querySelectorAll("tbody");
    console.log(tabBody);
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
    console.log(popup);
    var tab = document.querySelector("#tab11");
    console.log(tab);
    var tabBody = tab.querySelectorAll("tbody");
    console.log(tabBody);
    popup.querySelector("#file").value = tabBody[index].querySelector("#file").innerHTML;
    popup.querySelector("#date").value = tabBody[index].querySelector("#date").innerHTML;
    popup.querySelector("#aspid").value = tabBody[index].querySelector("#aspid").innerHTML;
    popup.querySelector("#id").value = tabBody[index].querySelector("#id").innerHTML;
}