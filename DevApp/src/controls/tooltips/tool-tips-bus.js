import MicroEvent   from "../../Libs/flux/Microevent.js";

class ToolTipsMessageBus{
    constructor() {
        this.ON_TOOLTIP_SHOW='onToolTipShow';
        this.ON_TOOLTIP_HIDE='onToolTipHide';
        this.ON_ALERT='onAlert';
    }
    hide(){
        this.trigger(this.ON_TOOLTIP_HIDE);
    }

    showXY(p,tips,delay){
        if(tips){
                let rect={x:p.clientX, y:p.clientY}    
                this.trigger(this.ON_TOOLTIP_SHOW,rect,tips,delay);
                }}
}

const bus = new ToolTipsMessageBus();
MicroEvent.mixin(bus);
bus.bind(bus.ON_ALERT,(p)=>{alert(p?p:"ON_ALERT")});
export default bus;