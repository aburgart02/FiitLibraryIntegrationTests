import {RubricSummary} from "src/models/RubricSummary";

export interface GroupedRubrics {
    parentRubric: RubricSummary;
    rubrics: RubricSummary[];
}
