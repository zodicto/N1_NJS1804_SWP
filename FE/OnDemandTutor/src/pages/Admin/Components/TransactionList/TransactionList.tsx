import { Table, TableColumnsType, Tooltip } from "antd";
import TransactionMenu from "../AdminMenu/TransactionMenu";
import { useState } from "react";
import { adminAPI } from "../../../../api/admin.api";
import { useQuery } from "@tanstack/react-query";
import { TransData } from "../../../../types/chart.type";

export default function TransactionList() {
    const { data: tranData = [], refetch } = useQuery<any>({
        queryKey: ['Request'],
        queryFn: () => adminAPI.getTransaction(),
        initialData: [] // Ensure tranData is always an array initially
    });
    console.log('tranData',tranData)

    const [searchText, setSearchText] = useState<string>('');
    console.log('searchText', searchText);

    const transformedData: TransData[] = tranData?.map((transaction: any) => ({
        idTran: transaction.id,
        createDate: transaction.createDate,
        status: transaction.status,
        amount: transaction.amount,
        userId: transaction.user.id,
        email: transaction.user.email,
        fullName: transaction.user.fullName,
        phone: transaction.user.phone,
        role: transaction.user.roles,
        address: transaction.user.address
    })) || [];

    const filteredData: TransData[] = transformedData.filter((item: TransData) => {
        const searchTextLower = searchText.toLowerCase();
        const idTranMatch = item.idTran ? item.idTran.toLowerCase().includes(searchTextLower) : false;
        const createDateMatch = item.createDate ? item.createDate.toLowerCase().includes(searchTextLower) : false;
        const statusMatch = item.status ? item.status.toLowerCase().includes(searchTextLower) : false;
        const amountMatch = item.amount ? item.amount.toString().toLowerCase().includes(searchTextLower) : false;
        const userIdMatch = item.userId ? item.userId.toLowerCase().includes(searchTextLower) : false;
        const emailMatch = item.email ? item.email.toLowerCase().includes(searchTextLower) : false;
        const fullNameMatch = item.fullName ? item.fullName.toLowerCase().includes(searchTextLower) : false;
        const phoneMatch = item.phone ? item.phone.toLowerCase().includes(searchTextLower) : false;
        const roleMatch = item.role ? item.role.toLowerCase().includes(searchTextLower) : false;
        const addressMatch = item.address ? item.address.toLowerCase().includes(searchTextLower) : false;

        return idTranMatch || createDateMatch || statusMatch || amountMatch || userIdMatch ||
               emailMatch || fullNameMatch || phoneMatch || roleMatch || addressMatch;
    });
    console.log('filteredData', filteredData);

    const columns: TableColumnsType<any> = [
        {
            title: 'Mã giao dịch',
            dataIndex: 'idTran',
            render: (text: string) => ( // tại vì text có thể null
                text ? (
                    <Tooltip title={text}>
                        {text.length > 20 ? `${text.slice(0, 20)}...` : text}
                    </Tooltip>
                ) : null
            ),
            onFilter: (value, record) =>
                record.fullName.indexOf(value as string) === 0,
            sorter: (a, b) => a.fullName.length - b.fullName.length,
            width: 200,
            fixed: 'left'
        },
        {
            title: 'Ngày giao dịch',
            dataIndex: 'createDate',
            defaultSortOrder: 'descend',
            width: 200
        },
        {
            title: 'Trạng thái',
            dataIndex: 'status',
            width: 200
        },
        {
            title: 'Số tiền nạp',
            dataIndex: 'amount',
            width: 150
        },
        {
            title: 'Id người nạp',
            dataIndex: 'userId',
            render: (text: string) => ( // tại vì text có thể null
                text ? (
                    <Tooltip title={text}>
                        {text.length > 20 ? `${text.slice(0, 20)}...` : text}
                    </Tooltip>
                ) : null
            ),
            width: 200
        },
        {
            title: 'Email',
            dataIndex: 'email',
            width: 150
        },
        {
            title: 'Số điện thoại',
            dataIndex: 'phone',
            width: 150
        },
        {
            title: '',
            dataIndex: 'role',
            width: 150
        },
        {
            title: 'Địa chỉ',
            dataIndex: 'address',
            width: 150
        }
    ];

    const handleSearch = (value: string) => {
        setSearchText(value);
        console.log(value);
    };

    const onChange = () => { } // Placeholder for future implementation

    return (
        <>
            <div className='text-left'>Danh sách giao dịch</div>
            <TransactionMenu
                list='list'
                req=''
                app=''
                rej=''
            />
            <div className='text-left shadow-sm shadow-black border-4 pt-5 h-[629px] rounded-t-xl mt-6'>
                <div className='mb-5'>
                    <input type="text" placeholder="Nhập từ khóa" onChange={(e) => handleSearch(e.target.value)} />
                </div>
                <Table
                    columns={columns}
                    dataSource={filteredData}
                    pagination={{ pageSize: 10 }}
                    onChange={onChange}
                    showSorterTooltip={{ target: 'sorter-icon' }}
                    scroll={{ x: 1300, y: 400 }}
                />
            </div>
        </>
    );
}
