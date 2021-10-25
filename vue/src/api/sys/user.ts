import { defHttp } from '/@/utils/http/axios';
import { LoginParams, LoginResultModel, GetUserInfoModel, LogoutParams } from './model/userModel';

import { useGlobSetting } from '/@/hooks/setting';

import { ErrorMessageMode } from '/#/axios';
import { ContentTypeEnum } from '/@/enums/httpEnum';

enum Api {
  Login = '/api/connect/token',
  Logout = '/api/connect/revocation',
  GetUserInfo = '/api/connect/userinfo',
  GetPermCode = '/getPermCode',
}

/**
 * @description: user login api
 */
export function loginApi(params: LoginParams, mode: ErrorMessageMode = 'modal') {
  const setting = useGlobSetting();
  const tokenParams = {
    client_id: setting.clientId,
    client_secret: setting.clientSecret,
    grant_type: 'password',
    username: params.username,
    password: params.password,
  };
  return defHttp.post<LoginResultModel>(
    {
      url: Api.Login,
      params: tokenParams,
      headers: {
        'Content-Type': ContentTypeEnum.FORM_URLENCODED,
      },
    },
    {
      errorMessageMode: mode,
      isTransformResponse: false,
    },
  );
}

/**
 * @description: getUserInfo
 */
export function getUserInfo() {
  return defHttp.get<GetUserInfoModel>(
    { url: Api.GetUserInfo },
    {
      errorMessageMode: 'none',
      isTransformResponse: false,
    },
  );
}

export function getPermCode() {
  return defHttp.get<string[]>({ url: Api.GetPermCode });
}

export function doLogout(params: LogoutParams) {
  const setting = useGlobSetting();
  const logoutParams = {
    client_id: setting.clientId,
    token: params.token,
    token_type_hint: 'access_token',
  };
  return defHttp.post(
    {
      url: Api.Logout,
      params: logoutParams,
      headers: {
        'Content-Type': ContentTypeEnum.FORM_URLENCODED,
      },
    },
    { isTransformResponse: false },
  );
}
