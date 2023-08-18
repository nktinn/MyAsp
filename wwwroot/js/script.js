//Details open/close

document.onclick = function (e) {
	var f = document.querySelector("details");
    if (!e.target.classList.contains("detail__body") && !e.target.classList.contains("detail__summary") && !e.target.classList.contains("detail__summary_img")) {
			if (f.open == true) {
				f.open = false;
        	}
    	}
    };