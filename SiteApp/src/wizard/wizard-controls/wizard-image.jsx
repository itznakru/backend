import React from 'react';
import "./wizard-controls.css";

function WizardImage({value, onRemoveImageValue, onUpload, error}) {
   
    const onLoadFile = (e) => {
        let files = e.target.files;
        var reader = new FileReader();
        let f = files[0];
        let name = f.name;
        let size = f.size;
        if (size > 5000000) {
            alert("Размер файла не может быть более 5 мб.")
            return;
        }

        reader.onload = (function (me) {
            return function (e) {
                if (onUpload)
                    onUpload({ fileBase64: e.target.result, size: size, name: name });
            }.bind(me);
        })(this);
        reader.readAsDataURL(f);
    }

    return (
        <div>
            <div className='wm-image'>
                {value ? 
                    <div>
                        <i className="fas fa-trash-alt" onClick={() => { onRemoveImageValue(); }}></i>
                        <img src={value}></img>
                    </div> 
                    : 
                    <label>
                         <input accept="image/*"
                            type="file"
                            autoComplete="off"
                            tabIndex="-1"
                            data-test="fileInput"
                            onChange={onLoadFile}
                        ></input> 
                        <p><span style={{ color: "red" }}>Кликните,</span> чтобы загрузить файл. PNG SVG JPG </p>
                    </label>
                }

            </div>
            <p className='wm-error-field'>{error}</p>
        </div>
    );
}

export default WizardImage;
