import React, {Component} from 'react';
import "./tooltips.css";
import ToolTipsBus from "./tool-tips-bus.js";

class ToolTipsClass extends Component {
    constructor(props) {
        super(props);
        this.state={
                visible:false,
                msg:"",
                x:0,
                y:0,
                timerid:0

        }
        this.myRef = React.createRef();
    }
    onShow(r,msg,delay){
        if(this.state.timerid!==0)
            clearTimeout(this.state.timerid);

        this.setState({visible:true,msg:msg, x:r.x,y:r.y +20});
        this.timerid=setTimeout(()=>{this.setState({visible:false})},delay||6000);
    }

    onHide(){
        this.setState({visible:false});
        clearTimeout(this.state.timerid);
        this.setState({timerid:0});
    }

    componentDidMount() {
        ToolTipsBus.bind(ToolTipsBus.ON_TOOLTIP_SHOW,this.onShow.bind(this));
        ToolTipsBus.bind(ToolTipsBus.ON_TOOLTIP_HIDE,this.onHide.bind(this));
    }
   
    onClick(){
        let v=this.state.checked;
        this.setState({checked:!v})
        if(this.props.onCheck)
                this.props.onCheck(!v);
    }
    render() {
            if(this.state.visible==false)
                return <></>;
            let tipStyle= {left:this.state.x,top:this.state.y};   
            let tipStyleSay={};

            if(window.innerWidth-this.state.x<200){
                tipStyle={left:this.state.x-170,top:this.state.y,width:300};
                tipStyleSay={marginLeft:165};
            }

            return( <div  ref={this.myRef} id="tipsId" className="tips" style={tipStyle}>
                         <div className="tips-say" style={tipStyleSay} ></div>
                        <div className="tips-body"><p>{this.state.msg}</p></div>                       
                    </div>
        )
        }
     
};

export default ToolTipsClass;