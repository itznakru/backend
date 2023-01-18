import React, { Component } from 'react';
import "../wizard-frame/wizard-frame.css";

class WizardImage extends Component {
    constructor(props){
        super(props);
    }
    renderValue(){
        return <div>
                    <i className="fas fa-trash-alt" onClick={()=>{this.props.onRemoveImageValue();}}></i>
                    <img src={this.props.value}></img>
                </div>
    }
    
    onLoadFile (e)
    {
        let files=e.target.files;
        var reader = new FileReader();
        let f = files[0];
        let name=f.name;
        let size = f.size;
        if (size > 5000000) {
                 alert("Размер файла не может быть более 5 мб.")
             return;
        }

        reader.onload = (function (me) {
            return function (e) {
                if(this.props.onUpload)
                        this.props.onUpload({fileBase64: e.target.result,size:size,name:name});
            }.bind(me);
        })(this);
        reader.readAsDataURL(f);
        
    }

    renderEmpty(){
        return  <label>
                    <input accept="image/*" 
                        type="file" 
                        autoComplete="off"  
                        tabIndex="-1" 
                        data-test="fileInput"
                        onChange={
                            this.onLoadFile.bind(this)}
                        ></input>
                    <p><span style={{color:"red"}}>Кликните,</span> чтобы загрузить файл. PNG SVG JPG </p>   
                </label>  
    }
    render() {
        return <div>
                <div className='wm-image'>
                       {this.props.value?this.renderValue():this.renderEmpty()}
                       
                </div>
                <p className='wm-error-field'>{this.props.error}</p>
              </div>  
    }
}
export default WizardImage;