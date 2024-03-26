import { GroupedRubrics } from "src/models/GroupedRubrics";
import { RubricSummary } from "src/models/RubricSummary";
import { handleRequest } from "src/api/HandleRequest";
import { ApiRoutes } from "./Api";

export const rubricApi = {
    select: async (): Promise<GroupedRubrics[]> => {
        return handleRequest(ApiRoutes.RubricSummarySelect);
    },
    get: async (synonym: string): Promise<RubricSummary> => {
        return handleRequest(`${ApiRoutes.RubricSummary}/${synonym}`);
    },
};
