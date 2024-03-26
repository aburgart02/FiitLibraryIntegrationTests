import { useHistory, useLocation } from "react-router-dom";
import { getJwt } from "src/utils/LocalStorageJWT";

export const useAuthentication = () => {
    const history = useHistory();
    const location = useLocation();

    const isAuthenticated = () => {
        const jwtToken = getJwt();
        return !!jwtToken;
    };

    if (!isAuthenticated() && location.pathname !== "/login" && location.pathname !== "/register") {
        history.push("/login");
    }

    return {
        history,
        location,
        isAuthenticated,
    };
}
