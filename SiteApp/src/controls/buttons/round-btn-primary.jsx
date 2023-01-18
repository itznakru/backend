//import React, {  useState, useEffect  } from "react";
import React, { Component }  from "react";
import "./buttons.css";
let ToolTipsBus=require("../tooltips/tool-tips-bus.js").default;

///
 class RoundBtnPrimary extends Component {
    constructor(props) {
        super(props);
        this.myRef = React.createRef();
    }

    componentDidMount() {
    }

    render() 
    {   
        let wideTipCss=null;
        if(this.props.tips.length>100)
             wideTipCss={width:'600px',marginLeft:'-300px'};  

        let tipSpan=null;
        if(this.props.tips)
            tipSpan=<span className="mytooltip">{this.props.name}</span>;

        let delBtn=<i className="far fa-times-circle" onClick={()=>{
                    if(this.props.onDelete){
                            ToolTipsBus.trigger(ToolTipsBus.ON_TOOLTIP_HIDE);   
                            this.props.onDelete({code:this.props.code,name:this.props.title})}
                            }}>
                    </i>

        if(this.props.onDelete==null)
            delBtn=null;
       
      

        return <div className="rb-w-del" 

                        ref={this.myRef}
                        onMouseOut={(p)=>{
                            ToolTipsBus.trigger(ToolTipsBus.ON_TOOLTIP_HIDE);                  
                        }
                        }
                        onMouseOver={(p)=>{
                            var rect=this.myRef.current.getBoundingClientRect();
                            if((p.pageY-rect.y)>100)
                                rect.y=p.pageY;
                            else
                                rect.y=rect.y+10;

                            ToolTipsBus.trigger(ToolTipsBus.ON_TOOLTIP_SHOW,rect,this.props.tips);
                        }}> 
                        {this.props.title} 
                        {delBtn}             
                </div>
    }
}

export default RoundBtnPrimary;
