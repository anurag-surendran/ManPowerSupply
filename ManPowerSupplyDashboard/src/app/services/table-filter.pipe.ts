import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'TableFilter'
})
export class TableFilterPipe implements PipeTransform {

  transform(items: any, filter: any, filterMetadata: any, sumCols?: string[], defaultFilter?: boolean): unknown {

    let result = items;
    if (!filter) {
      filterMetadata = this.setFilterMetadata(result, sumCols, filterMetadata);
      return result;
    }

    if (!Array.isArray(items)) {
      filterMetadata = this.setFilterMetadata(result, sumCols, filterMetadata);
      return result;
    }

    if (filter && Array.isArray(items)) {
      let filterKeys = Object.keys(filter); 

      if (defaultFilter) {
        result = items.filter(item =>
          filterKeys.reduce((x, keyName) =>
            (x && new RegExp(filter[keyName], 'gi').test(item[keyName])) || filter[keyName] == "", true));
      }
      else {
        result = items.filter(item => {
          return filterKeys.some((keyName) => {
            return new RegExp(filter[keyName], 'gi').test(item[keyName]) || filter[keyName] == "";
          });
        });
      }
      filterMetadata = this.setFilterMetadata(result, sumCols, filterMetadata);
      return result;
    }
  }

  setFilterMetadata(result: any, sumCols: string[], filterMetadata: any) {
    let sums: number[] = [];

    if (sumCols != null && sumCols != undefined && sumCols.length != 0)
      sumCols.forEach(x => sums.push(0));

    if (result == null || result == undefined || result.length == 0) {

      filterMetadata.count = 0;
      filterMetadata.sums = sums;
      return;
    }

    result.forEach((res: any) => {
      if (sumCols != null && sumCols != undefined && sumCols.length != 0)
        for (let index = 0; index < sumCols.length; index++) {
          sums[index] += res[sumCols[index]];
        }
    });

    filterMetadata.count = result.length;
    filterMetadata.sums = sums;
  }
} 