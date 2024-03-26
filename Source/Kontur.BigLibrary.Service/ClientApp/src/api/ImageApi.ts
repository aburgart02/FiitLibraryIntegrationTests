import { NotFoundError, ServerError, UnauthorizedError } from "src/api/Errors";
import { ApiRoutes } from "./Api";
import { getJwt, removeJwt } from "src/utils/LocalStorageJWT";

export async function handleUploadImage(inputUrl: string, formData: FormData): Promise<number> {
    const jwtToken = getJwt();

    const headers = new Headers({
        Authorization: `Bearer ${jwtToken}`,
    });

    const requestOptions = {
        method: "POST",
        headers,
        body: formData,
    };

    try {
        const response = await fetch(inputUrl, requestOptions);

        if (response.ok) {
            const data: number = await response.json();
            return data;
        } else if (response.status === 401) {
            removeJwt();
            throw new UnauthorizedError();
        } else if (response.status === 404) {
            throw new NotFoundError();
        } else {
            throw new ServerError();
        }
    } catch (error) {
        console.error("An error occurred:", error);
        throw error;
    }
}

const getImageUrl = (bookId: number, imageSize: string = "m"): string => {
    return `${ApiRoutes.Images}/${bookId}?size=${imageSize}`;
};

const uploadImage = async (data: FormData): Promise<number> => {
    return await handleUploadImage(ApiRoutes.Images, data);
};

export const imageApi = {
    getUrl: getImageUrl,
    upload: uploadImage,
};
