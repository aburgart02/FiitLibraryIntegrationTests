import * as React from "react";
import { api } from "src/api/Api";
import { Book } from "src/models/Book";
import { GroupedRubrics } from "src/models/GroupedRubrics";
import { RubricSummary } from "src/models/RubricSummary";

export const handleImageUpload = async (
    event: React.ChangeEvent<HTMLInputElement>,
    setBook: React.Dispatch<React.SetStateAction<Omit<Book, "id">>>,
    setImageError: React.Dispatch<React.SetStateAction<boolean>>,
): Promise<void> => {
    const File = event.target.files[0];
    setImageError(false);

    if (!File) {
        return;
    }

    if (File && !File.name.match(/\.(jpg)$/i)) {
        setImageError(true);
        return;
    }

    if (File && File.size > 150 * 1024) {
        setImageError(true);
        return;
    }

    const formData = new FormData();

    formData.append("File", File);

    const id = await api.image.upload(formData);

    setBook((prevBook) => ({
        ...prevBook,
        imageId: id,
    }));
};

export const loadRubrics = async (
    setBook: React.Dispatch<React.SetStateAction<Omit<Book, "id">>>,
    setRubrics: React.Dispatch<React.SetStateAction<RubricSummary[]>>,
): Promise<void> => {
    const groupedRubrics: GroupedRubrics[] = await api.rubric.select();

    groupedRubrics?.forEach?.(group => {
        setRubrics((state) => state.concat(group.rubrics));
    });
    setBook((prevBook) => ({
        ...prevBook,
        ["rubricId"]: groupedRubrics?.[0]?.rubrics[0]?.id ?? -1,
    }));
};

export const handleInputChange = (
    event: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>,
    setBook: React.Dispatch<React.SetStateAction<Omit<Book, "id">>>,
): void => {
    const { name, value } = event.currentTarget;
    const maxSafeInteger = 2147483647;

    const isNumber = /^[0-9]+$/.test(value);

    if (value && (name === "count" || name === "price") && !isNumber || value.length > 20 || +value > maxSafeInteger) {
        return;
    }

    setBook((prevBook) => ({
        ...prevBook,
        [name]: value,
    }));
};