import { BookFilter } from "src/models/BookFilter";
import LocalStorageUtils from "./LocalStorageUtils";

export const filterStorageKey = 'filter';

export const setFilter = (value: { showFree: boolean, filter: BookFilter }) => LocalStorageUtils.setItem<{ showFree: boolean, filter: BookFilter }>(filterStorageKey, value);
export const getFilter = (): { showFree: boolean, filter: BookFilter } | null => LocalStorageUtils.getItem<{ showFree: boolean, filter: BookFilter }>(filterStorageKey);
export const removeFilter = () => LocalStorageUtils.removeItem(filterStorageKey);
