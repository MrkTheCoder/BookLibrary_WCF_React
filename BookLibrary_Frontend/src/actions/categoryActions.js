import axios from "axios";
import {
  CATEGORY_LIST_REQUEST,
  CATEGORY_LIST_SUCCESS,
  CATEGORY_LIST_FAIL,
  //,
  ADD_FILTERS,
} from "../constants/categoryConstants";

export const categoryList = () => async (dispatch) => {
  try {
    dispatch({ type: CATEGORY_LIST_REQUEST });

    const { data } = await axios.get(`http://localhost:51203/api/categories`);

    dispatch({
      type: CATEGORY_LIST_SUCCESS,
      payload: data,
    });
  } catch (error) {
    dispatch({
      type: CATEGORY_LIST_FAIL,
      payload:
        error.response && error.response.data.Message
          ? error.response.data.Message
          : error.message,
    });
  }
};

export const addFilters = (filters) => async (dispatch) => {
  try {
    dispatch({
      type: ADD_FILTERS,
      payload: filters,
    });
  } catch (error) {
    dispatch({
      type: CATEGORY_LIST_FAIL,
      payload:
        error.response && error.response.data.Message
          ? error.response.data.Message
          : error.message,
    });
  }
};
