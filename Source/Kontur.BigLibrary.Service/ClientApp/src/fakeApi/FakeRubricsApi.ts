import {GroupedRubrics} from "src/models/GroupedRubrics";
import {RubricSummary} from "src/models/RubricSummary";
import {rubrics} from "src/fakeData/Rubrics";
import {rubric} from "src/fakeData/Rubric";

const getGroupedRubrics = (): Promise<GroupedRubrics[]> => {
    return new Promise(resolve => {
        setTimeout(() => resolve(rubrics), 500);
    });
};

const getRubric = (rubricSynonym: string): Promise<RubricSummary> => {
    return new Promise(resolve => {
        setTimeout(() => resolve(rubric), 500)
    });
};

//todo выпилить после правки бэка
const getRubricById = (id: number): Promise<RubricSummary> => {
    return new Promise(resolve => {
        setTimeout(() => resolve(rubric), 500)
    });
};

export const fakeRubricApi = {
    select: getGroupedRubrics,
    get: getRubric,
    getById: getRubricById,
};
