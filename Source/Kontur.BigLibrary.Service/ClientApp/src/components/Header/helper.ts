import { api } from "src/api/Api";

export const exportBooks = async () => {
    try {
        const response = await api.book.export();
        const blob = new Blob([response as string], { type: 'application/xml' });
        const blobUrl = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = blobUrl;
        link.setAttribute('download', 'books.xml');
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        window.URL.revokeObjectURL(blobUrl);
    } catch (error) {
        console.error('Ошибка при экспорте книг:', error);
    }
};
