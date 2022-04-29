const getPageAndItem = (address, type) => {
  const exp = /page=([0-9]+)&item=([0-9]+)/g;

  

  if (type == "nextPage") {
    let [_, page, item] = exp.exec(address);
    return { page: Number(page) - 1, item: item };
  } else if (type == "prevPage") {
    let [_, page, item] = exp.exec(address);
    return { page: Number(page) + 1, item: item };
  } else{
    return 0
  }
};

export default getPageAndItem;
