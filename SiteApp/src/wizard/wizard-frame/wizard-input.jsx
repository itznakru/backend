import React, { Component } from 'react';
import "../wizard-frame/wizard-frame.css"

class WizardInput extends Component {
    constructor(props){
        super(props);
    }

    renderOneRow(){
        return <div>
                <div className='wm-input'>
                    <input 
                        type="name" 
                        placeholder={this.props.placeholder} autocomplete="off" 
                        onChange={(p)=>{this.props.onChange(p.target.value)}}
                        value={this.props.value} required=""></input>
                </div>
                <p className='wm-error-field'>{this.props.error}</p>   
            </div>
    }

    renderMultiRow(){
        return <div>
                <div className='wm-input'>
                    <textarea 
                        cols="1"
                        rows="5"
                        type="name" 
                        placeholder={this.props.placeholder} 
                        autoComplete="off" 
                        onChange={(p)=>{this.props.onChange(p.target.value)}}
                        value={this.props.value} required=""></textarea>
                </div>
                <p className='wm-error-field'>{this.props.error}</p>   
            </div>
    }

    render() {
        return this.props.multiRow?this.renderMultiRow():this.renderOneRow(); 

    }
}
export default WizardInput;