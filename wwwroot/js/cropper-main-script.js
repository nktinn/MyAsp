var actionButton = document.getElementsByClassName('upload');



// upload image
document.onclick = function (cl)
{

    var f = document.querySelector("details");
    if (!cl.target.classList.contains("detail__body") && !cl.target.classList.contains("detail__summary") && !cl.target.classList.contains("detail__summary_img"))
    {
        if (f.open == true)
        {
            f.open = false;
        }
    }
    var eventer = document.getElementsByClassName(cl.target.className);
    if (eventer == actionButton)
    {
        {
            var hiddenUpload = document.getElementById('hidden-upload');
            var hiddensize = document.getElementById('hidden-size');
            hiddenUpload.click();
            // apdate on new file selected issue removed here
            hiddenUpload.onchange = function ()
            {
                var side_controls_shifter = document.querySelectorAll('.side-controls-shifter svg');
                var submitButton = document.getElementById('save_btn');
                var image_workspaceSpan = document.querySelector('.image-workspace span');
                var preview_containerSpan = document.querySelector('.preview-container span');
                document.querySelector('.image-workspace').innerHTML = `<img src="" alt="">`
                var image_workspace = document.querySelector('.image-workspace img');

                var file = hiddenUpload.files[0]
                const maxFileSizeInBytes = 5 * 1024 * 1024; //max size = 5mb
                if (file.size > maxFileSizeInBytes)
                {
                    alert('Файл слишком большой. Максимальный размер файла: 5 MB.');
                    hiddenUpload.value = ''; // Сбрасываем выбранный файл
                    file.value = '';
                }
                else
                {
                    var url = window.URL.createObjectURL(new Blob([file], { type: 'image/jpg' }))
                    image_workspace.src = url
                    image_workspaceSpan.style.display = 'none'
                    preview_containerSpan.style.display = 'none'

                    var options = {
                        dragMode: 'move',
                        preview: '.img-preview',
                        viewMode: 2,
                        modal: false,
                        background: false,
                        ready: function () {
                            // save cropped image
                            submitButton.onclick = () => {

                                const cropData = cropper.getData();
                                const lT = { x: cropData.x, y: cropData.y };
                                const rT = { x: cropData.x + cropData.width, y: cropData.y };
                                const lB = { x: cropData.x, y: cropData.y + cropData.height };
                                const rB = { x: cropData.x + cropData.width, y: cropData.y + cropData.height };

                                hiddensize.value = `${lT.x} ${lT.y} ${rT.x} ${rT.y} ${lB.x} ${lB.y} ${rB.x} ${rB.y}`;

                                document.getElementById('AddPhoto').submit();
                            }
                        }
                    }

                    var cropper = new Cropper(image_workspace, options)
                }
            }
        }
    }
}