function runPanel(){
    let memberKey='lg';
    let modes= ['wizard','search','calculator','account']; 
   
    let  loadJs=function(file,callback) {
        var script = document.createElement('script');
        script.src=file;
        script.type='text/javascript';
        script.onload=callback;
        document.getElementsByTagName('head')[0].appendChild(script);
    }
   
    let host = window.location.protocol + "//" + window.location.host;
    loadJs(host+'/app/wizard-plugin/wizard-help.js',()=>{
                          
                            loadcss(host+'/app/wizard-plugin/wizard-iframe.css');   
                            loadcss(host+'/app/partners/'+memberKey+'/skin_i_panel_'+memberKey+'.css');    
                            runStickyPanel(memberKey,modes)}
                );
}

runPanel();