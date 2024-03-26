import LocalStorageUtils from "./LocalStorageUtils";

export const JwtLocalStorageKey = 'jwtToken'

export const setJwt = (value: string) => LocalStorageUtils.setItem<string>(JwtLocalStorageKey, value);
export const getJwt = (): string | null => LocalStorageUtils.getItem<string>(JwtLocalStorageKey);
export const removeJwt = () => LocalStorageUtils.removeItem(JwtLocalStorageKey);
