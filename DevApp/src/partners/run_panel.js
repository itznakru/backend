function runPanel(){
    let memberKey=document.currentScript.getAttribute('memberKey'); ;
    let modes= ['wizard','search','calculator','account']; 
   
    let  loadJs=function(file,callback) {
        var script = document.createElement('script');
        script.src=file;
        script.type='text/javascript';
        script.onload=callback;
        document.getElementsByTagName('head')[0].appendChild(script);
    }
   
    let host ="https://www.tmservice.info/";
    if(window.location.host=="127.0.0.1:8080") 
        host=window.location.protocol + "//" + window.location.host;

    if( window.isWizardHelpLoaded){
        loadcss(host+'/app/wizard-plugin/wizard-iframe.css');   
        loadcss(host+'/app/partners/'+memberKey+'/skin_i_panel_'+memberKey+'.css');    
        runStickyPanel(memberKey,modes)
    }
        else
        loadJs(host+'/app/wizard-plugin/wizard-help.js',()=>{
                            window.isWizardHelpLoaded=true;
                            loadcss(host+'/app/wizard-plugin/wizard-iframe.css');   
                            loadcss(host+'/app/partners/'+memberKey+'/skin_i_panel_'+memberKey+'.css');    
                            runStickyPanel(memberKey,modes)
                        }
                );
}
runPanel();