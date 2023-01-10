function runPartner(){
    debugger;
    let memberKey='test';
    let modes= ['wizard','search','calculator','account']; 
    let btnId=document.currentScript.getAttribute('btnId');
    
    let  loadJs=function(file,callback) {
        var script = document.createElement('script');
        script.src=file;
        script.type='text/javascript';
        script.onload=callback;
        document.getElementsByTagName('head')[0].appendChild(script);
    }
    
    let host = window.location.protocol + "//" + window.location.host;
    if( window.isWizardHelpLoaded){
            runPartnerWizard(memberKey,btnId);
        }
        else
        loadJs(host+'/app/wizard-plugin/wizard-help.js',()=>{
                            window.isWizardHelpLoaded=true;
                            loadcss(host+'/app/wizard-plugin/wizard-iframe.css');   
                            loadcss(host+'/app/partners/'+memberKey+'/skin_panel_'+memberKey+'.css');    
                            runPartnerWizard(memberKey,btnId);
                        }
                );
       
}
runPartner();