import * as React from "react";
import DropdownMenu from "@skbkontur/react-ui/DropdownMenu";
import MenuItem from "@skbkontur/react-ui/MenuItem";
import { User } from "src/models/User";
import { ArrowTriangleDown, Logout } from "@skbkontur/react-icons";
import { api } from "src/api/Api";
import { removeJwt } from "src/utils/LocalStorageJWT";

const styles = require("./CurrentUser.less");

interface CurrentUserProps {
    user: User;
}

const logout = async (): Promise<void> => {
    removeJwt();
    await api.user.logout();
    window.location.reload();
};

export const CurrentUser: React.FC<CurrentUserProps> = ({ user }) => {
    return user && (
        <DropdownMenu
            caption={
                <div data-tid="current_user_menu" className={styles.avatarWrapper}>
                    <div className={styles.avatar}>
                        {(user.name ?? "").toUpperCase()[0]}
                    </div>
                    &nbsp;
                    <ArrowTriangleDown size={16} color="#808080" />
                </div>
            }
            menuWidth="180px"
            positions={["bottom right"]}
        >
            <MenuItem icon={<Logout />} onClick={logout}>
            <div  data-tid="LogOut"> Выйти </div>
            </MenuItem>
        </DropdownMenu>
    );
};
