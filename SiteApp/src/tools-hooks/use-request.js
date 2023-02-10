
import { appSettings } from './../AppSettings'
import ToolHelper from './../helpers/format-tools'


const callRequest = (methodType, controller, method = '', p, cbOk, cbBusinessExcp) => {

    const h = new ToolHelper();
    let _httpResponse = undefined
    // // critcall error handler
    const onError = (err) => {
        if (_httpResponse.status === 500) {
            console.log(err)
            window.location.href = appSettings.ERROR_PAGE
            return
        }

        cbBusinessExcp && cbBusinessExcp(err, _httpResponse)
    }

    //call critical error or continue
    const prepareResponseData = (response) => {
        _httpResponse = response;
        const respBody = response.json();
        // if normall process continue 
        if (_httpResponse.status === 200)
            return respBody;
        else
            // exception or business exception
            respBody.then((bExcp) => {
                onError(bExcp)
            })
    }

    const prepareUrl = (p) => {
        let m = h.isEmpty(method) ? '/' : '/' + method + '/?';
        let r = `${appSettings.REST_SERVERURL}${controller}${m}`;
        if (methodType == 'GET')
            r = r + new URLSearchParams(p);
        return r;
    }
    // params always have memberKey
    const prepareParams = () => {
        let memberKey = h.getUrlParameter('memberKey');
        return h.isEmpty(p) ? { memberKey: memberKey } : { ...p, memberKey: memberKey }
    }

    let _params = prepareParams();
    let _url = prepareUrl(_params);
    let _body = (methodType == 'POST') ? JSON.stringify(_params) : undefined

    fetch(_url, {
        method: methodType,
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: _body
    })
        .then(prepareResponseData)
        .then((p) => _httpResponse.status == 200 && cbOk(p))
    //   .catch(onError)
}

const FetchGet = (controller, method, p, cbOk, cbBusinessExcp) => { callRequest('GET', controller, method, p, cbOk, cbBusinessExcp) }
const FetchPost = (controller, method, p, cbOk, cbBusinessExcp) => { callRequest('POST', controller, method, p, cbOk, cbBusinessExcp) }

export { FetchGet, FetchPost }