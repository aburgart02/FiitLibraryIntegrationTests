export default class LocalStorageUtils {
    static setItem<T>(key: string, value: T): void {
        try {
            localStorage.setItem(key, JSON.stringify(value));
        } catch (error) {
            console.error("Ошибка при сохранении данных в Local Storage:", error);
        }
    }

    static getItem<T>(key: string): T | null {
        try {
            const data = localStorage.getItem(key);
            return data ? JSON.parse(data) : null;
        } catch (error) {
            console.error("Ошибка при получении данных из Local Storage:", error);
            return null;
        }
    }

    static removeItem(key: string): void {
        try {
            localStorage.removeItem(key);
        } catch (error) {
            console.error("Ошибка при удалении данных из Local Storage:", error);
        }
    }
}
