using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;

namespace THOK.Wms.Bll.Interfaces
{
    public interface ICellService : IService<Cell>
    {
        object GetDetails(int page, int rows, string cellCode);

        bool Add(Cell cell);

        bool Delete(string cellCode);

        bool Save(Cell cell);

        object GetWareCheck(string shelfCode);

        object GetSearch(string wareCode);

        object FindCell(string parameter);

        object GetCell(string shelfCode);


        object GetMoveCellDetails(string shelfCode, string inOrOut, string productCode);

        bool SaveCell(string wareCodes, string areaCodes, string shelfCodes, string cellCodes, string defaultProductCode, string editType);


        object GetCellInfo();

        object GetCellInfo(string productCode);

        object GetSortCell(string areaType);

        object GetCellCheck(string productCode);

        bool DeleteCell(string productCodes);

        bool SetTree2(string strId, string proCode);

        object GetCellBy(int page, int rows, string QueryString, string Value);
    }
}
