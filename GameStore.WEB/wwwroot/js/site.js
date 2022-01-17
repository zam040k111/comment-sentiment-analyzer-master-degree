
function ChangeTegClass(el, className) { el.setAttribute("class", className); }

function ChangeQuantity(form, index) {
    const xhr = new XMLHttpRequest();
    xhr.open("get", `order/changeQuantity?itemIndex=${index}&newQuantity=${form.value}`, true);
    xhr.onload = function () {
        const el = document.getElementById("TotalPrice");
        el.textContent = `Total price: ${Math.round(this.response).toFixed(2)}$`;
    }
    xhr.send();
}

function CheckUpdateIsAllowed(id, controllerName, useControllerName = true) {
    const xhr = new XMLHttpRequest();

    if (useControllerName) {
        xhr.open("get", `${controllerName}/CheckUpdateIsAllowed?id=${id}`, false);
    } else {
        xhr.open("get", `CheckUpdateIsAllowed?id=${id}`, false);
    }
    xhr.send();

    if (xhr.status === 200) {
        if (xhr.response.length > 0) {
            alert(`This ${controllerName} has been added to games with key "${xhr.response}". You will be able to change the ${controllerName} if you remove it from all games.`);
            return false;
        }
    }

    return true;
}

function ChangePage(pageNumber) {
    document.getElementById("PageNumber").value = pageNumber;
    document.getElementById("form").submit();
}
