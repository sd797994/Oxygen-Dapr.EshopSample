/**
 * 解析url参数
 * @example ?id=123&name=jack
 * @return Object {id:12345,name:jack}
 */
export function urlParse() {
  let url = window.location.search;
  let obj = {};
  let reg = /[?&][^?&]+=[^?&]+/g;
  let arr = url.match(reg);
  // ['?id=123',&name=jack]
  if (arr) {
    arr.forEach((item) => {
      let tempArr = item.substring(1).split('=');
      let key = decodeURIComponent(tempArr[0]);
      let val = decodeURIComponent(tempArr[1]);
      obj[key] = val;
    });
  }
  return obj;
};

export function saveToLocal(id, key, value) {
  if (!window.localStorage) {
    return false;
  } else {
    let store = window.localStorage._store_;
    if (!store) {
      store = {};
      store[id] = {};
    } else {
      store = JSON.parse(store);
      if (!store[id]) {
        store[id] = {};
      }
    }
    store[id][key] = value;
    window.localStorage._store_ = JSON.stringify(store);
  }
};

export function loadFormLocal(id, key, def) {
  if (!window.localStorage) {
    return false;
  } else {
    let store = window.localStorage._store_;
    if (!store) {
      return def;
    }
    store = JSON.parse(store)[id];
    if (!store) {
      return def;
    }
    let ret = store[key];
    return ret || def;
  }
};

