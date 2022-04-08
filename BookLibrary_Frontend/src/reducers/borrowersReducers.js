import {
  BORROWER_LIST_FAIL,
  BORROWER_LIST_REQUEST,
  BORROWER_LIST_SUCCESS,
} from "../constants/borrowersConstants";

export const borrowerListReducer = (state = { borrowers: [] }, action) => {
  switch (action.type) {
    case BORROWER_LIST_REQUEST:
      return { loading: true, borrowers: [] };
    case BORROWER_LIST_SUCCESS:
      return {
        loading: false,
        borrowers: action.payload,
        headers: action.headers,
      };
    case BORROWER_LIST_FAIL:
      return { loading: false, error: action.payload };
    default:
      return state;
  }
};
