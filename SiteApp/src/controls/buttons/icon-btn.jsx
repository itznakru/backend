import React, { Component } from 'react';
import ToolTipsBus from "../tooltips/tool-tips-bus.js"
import "./buttons.css"

class IconBtn extends Component {
    render() {
        const {onClick,icon,tips}=this.props;
        return (
            <div className="icon-btn" onClick={onClick}>
                <i  className={icon} 
                    onMouseOver={(p)=>{
                        tips && ToolTipsBus.showXY(p,tips)}}
                    onMouseOut={(p)=>{
                        tips && ToolTipsBus.hide()}}>
                </i>
            </div>
        );
    }
}

export default IconBtn;