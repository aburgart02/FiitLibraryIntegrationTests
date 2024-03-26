import * as React from "react";
import Link from "@skbkontur/react-ui/Link";
import Telegram2Icon from "@skbkontur/react-icons/Telegram2";
import EmailIcon from "@skbkontur/react-icons/Mail";
import DeviceSmartphoneIcon from "@skbkontur/react-icons/DeviceSmartphone";
import {Contact} from "src/models/Librarian";

interface LibrarianContactProps {
    contact: Contact;
    gray?: boolean;
}

export function LibrarianContact({contact, gray}: LibrarianContactProps): React.ReactElement {
    if (contact === undefined) {
        return null;
    }
    switch (contact.type) {
        case "telegram":
            return (
                <Link
                    use={gray ? "grayed" : "default"}
                    href={`tg://resolve?domain=${contact.value}`}
                    icon={<Telegram2Icon size={14}/>}>
                    {contact.value}
                </Link>
            );
        case "email":
            return (
                <Link
                    use={gray ? "grayed" : "default"}
                    href={`mailto:${contact.value}`}
                    icon={<EmailIcon size={14} />}>
                    {contact.value}
                </Link>
            );
        case "phone":
            return (
                <>
                    <DeviceSmartphoneIcon size={14}/>
                    {contact.value}
                </>
            );
        default:
            return null;
    }
}