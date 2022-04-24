import {
  CATEGORY_LIST_REQUEST,
  CATEGORY_LIST_SUCCESS,
  CATEGORY_LIST_FAIL,
  //
  ADD_FILTERS,
  RESET_FILTERS,
} from "../constants/categoryConstants";

export const categoryListReducer = (state = { categories: [] }, action) => {
  switch (action.type) {
    case CATEGORY_LIST_REQUEST:
      return { loading: true, categories: [] };

    case CATEGORY_LIST_SUCCESS:
      return { loading: false, categories: action.payload };

    case CATEGORY_LIST_FAIL:
      return { loading: false, error: action.payload };
    default:
      return state;
  }
};

export const filtersReducer = (state = {}, action) => {
  switch (action.type) {
    case ADD_FILTERS:
      return { success: true, filters: action.payload };

    case RESET_FILTERS:
      return {
        filters: {
          item: 10,
        },
      };

    default:
      return state;
  }
};
