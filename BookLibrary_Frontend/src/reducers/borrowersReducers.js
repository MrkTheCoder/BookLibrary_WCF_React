import {
  BORROWER_LIST_FAIL,
  BORROWER_LIST_REQUEST,
  BORROWER_LIST_SUCCESS,
  //
  BORROWER_DETAILS_REQUEST,
  BORROWER_DETAILS_FAIL,
  BORROWER_DETAILS_SUCCESS,
  BORROWER_DETAILS_RESET,
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
export const borrowerDetailsReducer = (state = {}, action) => {
  switch (action.type) {
    case BORROWER_DETAILS_REQUEST:
      return { loading: true, ...state };
    case BORROWER_DETAILS_SUCCESS:
      return {
        loading: false,
        borrower: action.payload,
      };
    case BORROWER_DETAILS_FAIL:
      return { loading: false, error: action.payload };
    case BORROWER_DETAILS_RESET:
      return {};
    case BORROWER_DETAILS_REQUEST:
      return {};
    default:
      return state;
  }
};
