For nRowIndex = 0 To EInvBillDetGrid.RowCount - 1
    If EInvBillDetGrid.Rows(nRowIndex).Cells(0).Value = True Then
        'And EInvBillDetGrid.Rows(nRowIndex).Cells(1).Value <> True 
        'MarkProcessed()
        If nDebug = 0 Then
            MsgBox("Generating IRN for Bill No." + EInvBillDetGrid.Rows(nRowIndex).Cells("DocDtls_No").Value.Trim())
        End If

        txtErrMsg.Text = "Please wait while generating E-Inv No. for Bill No.: " + EInvBillDetGrid.Rows(nRowIndex).Cells("DocDtls_No").Value.Trim()
        WriteToErrorLog(txtErrMsg.Text + " RowIndex " + nRowIndex)
        If nDebug = 0 Then
            MsgBox("1")
        End If
        cBillNo = EInvBillDetGrid.Rows(nRowIndex).Cells("DocDtls_No").Value.Trim()
        cInvId = EInvBillDetGrid.Rows(nRowIndex).Cells("InvId").Value.Trim()

        If nDebug = 0 Then
            MsgBox("2")
        End If

        eInvGen = New ReqPlGenIRN()
        eInvGen.Version = EInvBillDetGrid.Rows(nRowIndex).Cells("Version").Value.Trim()

        eInvGen.TranDtls = New ReqPlGenIRN.TranDetails()
        eInvGen.TranDtls.TaxSch = "GST"  'EInvBillDetGrid.Rows(nRowIndex).Cells("TranDtls_TaxSch").Value.Trim()
        eInvGen.TranDtls.SupTyp = EInvBillDetGrid.Rows(nRowIndex).Cells("TranDtls_SupTyp").Value.Trim()
        eInvGen.TranDtls.RegRev = "N"
        eInvGen.TranDtls.EcmGstin = Nothing
        eInvGen.TranDtls.IgstOnIntra = "N"

        If nDebug = 0 Then
            MsgBox("3")
        End If

        eInvGen.DocDtls = New ReqPlGenIRN.DocSetails()
        eInvGen.DocDtls.Typ = EInvBillDetGrid.Rows(nRowIndex).Cells("DocDtls_Typ").Value.Trim()
        eInvGen.DocDtls.No = EInvBillDetGrid.Rows(nRowIndex).Cells("DocDtls_No").Value.Trim()
        eInvGen.DocDtls.Dt = EInvBillDetGrid.Rows(nRowIndex).Cells("DocDtls_Dt").Value.Trim()
        If nDebug = 0 Then
            MsgBox("4")
        End If

        eInvGen.SellerDtls = New ReqPlGenIRN.SellerDetails()
        eInvGen.SellerDtls.Gstin = EInvBillDetGrid.Rows(nRowIndex).Cells("SellerDtls_Gstin").Value.Trim()
        eInvGen.SellerDtls.LglNm = EInvBillDetGrid.Rows(nRowIndex).Cells("SellerDtls_LglNm").Value.Trim()
        eInvGen.SellerDtls.TrdNm = EInvBillDetGrid.Rows(nRowIndex).Cells("SellerDtls_TrdNm").Value.Trim()
        eInvGen.SellerDtls.Addr1 = EInvBillDetGrid.Rows(nRowIndex).Cells("SellerDtls_Addr1").Value.Trim()
        If EInvBillDetGrid.Rows(nRowIndex).Cells("SellerDtls_Addr2").Value.Trim() = "" Then
            eInvGen.SellerDtls.Addr2 = Nothing
        Else
            eInvGen.SellerDtls.Addr2 = EInvBillDetGrid.Rows(nRowIndex).Cells("SellerDtls_Addr2").Value.Trim()
        End If

        eInvGen.SellerDtls.Loc = EInvBillDetGrid.Rows(nRowIndex).Cells("SellerDtls_Loc").Value.Trim()
        eInvGen.SellerDtls.Pin = EInvBillDetGrid.Rows(nRowIndex).Cells("SellerDtls_Pin").Value.Trim()
        eInvGen.SellerDtls.Stcd = RetStateCode(EInvBillDetGrid.Rows(nRowIndex).Cells("SellerDtls_State").Value.Trim())
        eInvGen.SellerDtls.Ph = Nothing 'EInvBillDetGrid.Rows(nRowIndex).Cells("SellerDtls_Ph").Value.Trim()
        eInvGen.SellerDtls.Em = Nothing 'EInvBillDetGrid.Rows(nRowIndex).Cells("SellerDtls_Em").Value.Trim()
        If nDebug = 0 Then
            MsgBox("5")
        End If

        eInvGen.BuyerDtls = New ReqPlGenIRN.BuyerDetails()
        eInvGen.BuyerDtls.Gstin = EInvBillDetGrid.Rows(nRowIndex).Cells("BuyerDtls_Gstin").Value.Trim()
        eInvGen.BuyerDtls.LglNm = EInvBillDetGrid.Rows(nRowIndex).Cells("BuyerDtls_LglNm").Value.Trim()
        eInvGen.BuyerDtls.TrdNm = EInvBillDetGrid.Rows(nRowIndex).Cells("BuyerDtls_TrdNm").Value.Trim()
        eInvGen.BuyerDtls.Pos = EInvBillDetGrid.Rows(nRowIndex).Cells("BuyerDtls_Pos").Value.Trim()
        eInvGen.BuyerDtls.Addr1 = EInvBillDetGrid.Rows(nRowIndex).Cells("BuyerDtls_Addr1").Value.Trim()

        If EInvBillDetGrid.Rows(nRowIndex).Cells("BuyerDtls_Addr2").Value.Trim() = "" Then
            eInvGen.BuyerDtls.Addr2 = Nothing
        Else
            eInvGen.BuyerDtls.Addr2 = EInvBillDetGrid.Rows(nRowIndex).Cells("BuyerDtls_Addr2").Value.Trim()
        End If

        eInvGen.BuyerDtls.Loc = EInvBillDetGrid.Rows(nRowIndex).Cells("BuyerDtls_Loc").Value.Trim()
        eInvGen.BuyerDtls.Pin = EInvBillDetGrid.Rows(nRowIndex).Cells("BuyerDtls_Pin").Value.Trim()

        If nDebug = 0 Then
            MsgBox(EInvBillDetGrid.Rows(nRowIndex).Cells("BuyerDtls_State").Value.Trim())
            MsgBox(RetStateCode(EInvBillDetGrid.Rows(nRowIndex).Cells("BuyerDtls_State").Value.Trim()))
        End If

        eInvGen.BuyerDtls.Stcd = RetStateCode(EInvBillDetGrid.Rows(nRowIndex).Cells("BuyerDtls_State").Value.Trim())

        If nDebug = 0 Then
            MsgBox("6")
        End If

        If EInvBillDetGrid.Rows(nRowIndex).Cells("DispDtls_Nm").Value.Trim() <> "" Then
            If nDebug = 0 Then
                MsgBox("61")
            End If

            eInvGen.DispDtls = New ReqPlGenIRN.DispatchedDetails()
            eInvGen.DispDtls.Nm = EInvBillDetGrid.Rows(nRowIndex).Cells("DispDtls_Nm").Value.Trim()
            eInvGen.DispDtls.Addr1 = EInvBillDetGrid.Rows(nRowIndex).Cells("DispDtls_Addr1").Value.Trim()
            eInvGen.DispDtls.Addr2 = EInvBillDetGrid.Rows(nRowIndex).Cells("DispDtls_Addr2").Value.Trim()
            eInvGen.DispDtls.Loc = EInvBillDetGrid.Rows(nRowIndex).Cells("DispDtls_Loc").Value.Trim()
            eInvGen.DispDtls.Pin = EInvBillDetGrid.Rows(nRowIndex).Cells("DispDtls_Pin").Value.Trim()
            eInvGen.DispDtls.Stcd = EInvBillDetGrid.Rows(nRowIndex).Cells("DispDtls_Stcd").Value.Trim()
        Else
            eInvGen.DispDtls = Nothing
        End If

        If nDebug = 0 Then
            MsgBox("7")
            MsgBox(JsonConvert.SerializeObject(eInvGen))
        End If

        If EInvBillDetGrid.Rows(nRowIndex).Cells("ShipDtls_Gstin").Value.trim() <> "" Then
            eInvGen.ShipDtls = New ReqPlGenIRN.ShippedDetails()
            eInvGen.ShipDtls.Gstin = EInvBillDetGrid.Rows(nRowIndex).Cells("ShipDtls_Gstin").Value.Trim()
            eInvGen.ShipDtls.LglNm = EInvBillDetGrid.Rows(nRowIndex).Cells("ShipDtls_LglNm").Value.Trim()
            eInvGen.ShipDtls.TrdNm = EInvBillDetGrid.Rows(nRowIndex).Cells("ShipDtls_TrdNm").Value.Trim()
            eInvGen.ShipDtls.Addr1 = EInvBillDetGrid.Rows(nRowIndex).Cells("ShipDtls_Addr1").Value.Trim()

            If EInvBillDetGrid.Rows(nRowIndex).Cells("ShipDtls_Addr2").Value.Trim() = "" Then
                eInvGen.ShipDtls.Addr2 = Nothing
            Else
                eInvGen.ShipDtls.Addr2 = EInvBillDetGrid.Rows(nRowIndex).Cells("ShipDtls_Addr2").Value.Trim()
            End If

            eInvGen.ShipDtls.Loc = EInvBillDetGrid.Rows(nRowIndex).Cells("ShipDtls_Loc").Value.Trim()
            eInvGen.ShipDtls.Pin = EInvBillDetGrid.Rows(nRowIndex).Cells("ShipDtls_Pin").Value.Trim()
            eInvGen.ShipDtls.Stcd = EInvBillDetGrid.Rows(nRowIndex).Cells("ShipDtls_StCd").Value.Trim()
        Else
            eInvGen.ShipDtls = Nothing
        End If

        If nDebug = 0 Then
            MsgBox("8")
        End If


        eInvGen.PayDtls = Nothing
        eInvGen.RefDtls = Nothing
        eInvGen.AddlDocDtls = Nothing
        eInvGen.ExpDtls = Nothing

        eInvGen.ValDtls = New ReqPlGenIRN.ValDetails()
        eInvGen.ValDtls.AssVal = Convert.ToDecimal(EInvBillDetGrid.Rows(nRowIndex).Cells("ValDtls_AssVal").Value)

        eInvGen.ValDtls.CgstVal = Convert.ToDecimal(EInvBillDetGrid.Rows(nRowIndex).Cells("ValDtls_CgstVal").Value)
        eInvGen.ValDtls.SgstVal = Convert.ToDecimal(EInvBillDetGrid.Rows(nRowIndex).Cells("ValDtls_SgstVal").Value)
        eInvGen.ValDtls.IgstVal = Convert.ToDecimal(EInvBillDetGrid.Rows(nRowIndex).Cells("ValDtls_IgstVal").Value)
        eInvGen.ValDtls.CesVal = Convert.ToDecimal(EInvBillDetGrid.Rows(nRowIndex).Cells("ValDtls_CesVal").Value)
        eInvGen.ValDtls.StCesVal = Convert.ToDecimal(EInvBillDetGrid.Rows(nRowIndex).Cells("ValDtls_StCesVal").Value)
        eInvGen.ValDtls.RndOffAmt = Convert.ToDecimal(EInvBillDetGrid.Rows(nRowIndex).Cells("ValDtls_RndOffAmt").Value)
        eInvGen.ValDtls.OthChrg = Convert.ToDecimal(EInvBillDetGrid.Rows(nRowIndex).Cells("ValDtls_OthChrg").Value)
        eInvGen.ValDtls.TotInvVal = Convert.ToDecimal(EInvBillDetGrid.Rows(nRowIndex).Cells("ValDtls_TotInvVal").Value)
        If nDebug = 0 Then
            MsgBox("9")
        End If

        eInvGen.EwbDtls = Nothing


        eInvGen.ItemList = New List(Of ReqPlGenIRN.ItmList)()
        Dim itm As ReqPlGenIRN.ItmList = New ReqPlGenIRN.ItmList()

        If nDebug = 0 Then
            MsgBox("12")
        End If

        itm.SlNo = EInvBillDetGrid.Rows(nRowIndex).Cells("itm_SlNo").Value.Trim()
        itm.IsServc = EInvBillDetGrid.Rows(nRowIndex).Cells("itm_IsServc").Value.Trim()
        itm.PrdDesc = EInvBillDetGrid.Rows(nRowIndex).Cells("itm_PrdDesc").Value.Trim()

        itm.HsnCd = EInvBillDetGrid.Rows(nRowIndex).Cells("itm_HsnCd").Value.Trim()
        If nDebug = 0 Then
            MsgBox("13")
        End If

        itm.BchDtls = Nothing 'EInvBillDetGrid.Rows(nRowIndex).Cells("itm_BchDtls").Value.Trim()
        itm.Qty = Convert.ToDecimal(EInvBillDetGrid.Rows(nRowIndex).Cells("itm_Qty").Value)
        itm.Unit = EInvBillDetGrid.Rows(nRowIndex).Cells("itm_Unit").Value.Trim()
        If nDebug = 0 Then
            MsgBox("14")
        End If

        itm.UnitPrice = Convert.ToDecimal(EInvBillDetGrid.Rows(nRowIndex).Cells("itm_UnitPrice").Value)
        itm.TotAmt = Convert.ToDecimal(EInvBillDetGrid.Rows(nRowIndex).Cells("itm_TotAmt").Value)
        itm.Discount = Convert.ToDecimal(EInvBillDetGrid.Rows(nRowIndex).Cells("itm_Discount").Value)
        If nDebug = 0 Then
            MsgBox("15")
        End If

        itm.AssAmt = Convert.ToDecimal(EInvBillDetGrid.Rows(nRowIndex).Cells("itm_AssAmt").Value)
        itm.GstRt = Convert.ToDecimal(EInvBillDetGrid.Rows(nRowIndex).Cells("itm_GstRt").Value)
        If nDebug = 0 Then
            MsgBox("152")
        End If
        itm.SgstAmt = Convert.ToDecimal(EInvBillDetGrid.Rows(nRowIndex).Cells("itm_SgstAmt").Value)
        If nDebug = 0 Then
            MsgBox("153")
        End If
        itm.IgstAmt = Convert.ToDecimal(EInvBillDetGrid.Rows(nRowIndex).Cells("itm_IgstAmt").Value)
        If nDebug = 0 Then
            MsgBox("154")
        End If

        itm.CgstAmt = Convert.ToDecimal(EInvBillDetGrid.Rows(nRowIndex).Cells("itm_CgstAmt").Value)
        If nDebug = 0 Then
            MsgBox("155")
        End If

        itm.CesRt = Convert.ToDecimal(EInvBillDetGrid.Rows(nRowIndex).Cells("itm_CesRt").Value)
        If nDebug = 0 Then
            MsgBox("156")
        End If

        itm.CesAmt = Convert.ToDecimal(EInvBillDetGrid.Rows(nRowIndex).Cells("itm_CesAmt").Value)
        If nDebug = 0 Then
            MsgBox("16")
        End If

        itm.CesNonAdvlAmt = Convert.ToDecimal(EInvBillDetGrid.Rows(nRowIndex).Cells("itm_CesNonAdvlAmt").Value)
        itm.StateCesRt = Convert.ToDecimal(EInvBillDetGrid.Rows(nRowIndex).Cells("itm_StateCesRt").Value)
        itm.StateCesAmt = Convert.ToDecimal(EInvBillDetGrid.Rows(nRowIndex).Cells("itm_StateCesAmt").Value)
        itm.StateCesNonAdvlAmt = Convert.ToDecimal(EInvBillDetGrid.Rows(nRowIndex).Cells("itm_StateCesNonAdvlAmt").Value)
        itm.OthChrg = Convert.ToDecimal(EInvBillDetGrid.Rows(nRowIndex).Cells("itm_OthChrg").Value)
        If nDebug = 0 Then
            MsgBox("17")
        End If

        itm.TotItemVal = Convert.ToDecimal(EInvBillDetGrid.Rows(nRowIndex).Cells("itm_TotItemVal").Value)
        itm.AttribDtls = Nothing
        If nDebug = 0 Then
            MsgBox("10")
        End If
        If nDebug = 0 Then
            MsgBox(JsonConvert.SerializeObject(itm))
        End If

        eInvGen.ItemList.Add(itm)

        If nDebug = 0 Then
            MsgBox(JsonConvert.SerializeObject(eInvGen))
        End If

        If nDebug = 0 Then
            MsgBox("3")
        End If

        nRowIndex = nRowIndex + 1
        nRowCtr = nRowIndex

        For nItemCtr = nRowCtr To EInvBillDetGrid.RowCount - 1
            If nDebug = 0 Then
                MsgBox("18")
            End If

            If cBillNo = EInvBillDetGrid.Rows(nItemCtr).Cells("DocDtls_No").Value.Trim() And cInvId = EInvBillDetGrid.Rows(nItemCtr).Cells("InvId").Value.Trim() Then
                'itm.SlNo = EInvBillDetGrid.Rows(nRowIndex).Cells("itm_SlNo").Value.Trim()

                itm = New ReqPlGenIRN.ItmList()

                If nDebug = 0 Then
                    MsgBox("19")
                End If
                itm.SlNo = Convert.ToInt32(EInvBillDetGrid.Rows(nItemCtr).Cells("itm_SlNo").Value.Trim()).ToString()
                itm.IsServc = EInvBillDetGrid.Rows(nItemCtr).Cells("itm_IsServc").Value.Trim()
                If nDebug = 0 Then
                    MsgBox("20")
                End If

                itm.PrdDesc = EInvBillDetGrid.Rows(nItemCtr).Cells("itm_PrdDesc").Value.Trim()
                itm.HsnCd = EInvBillDetGrid.Rows(nItemCtr).Cells("itm_HsnCd").Value.Trim()
                itm.BchDtls = Nothing 'EInvBillDetGrid.Rows(nItemCtr).Cells("itm_BchDtls").Value.Trim()
                If nDebug = 0 Then
                    MsgBox("21")
                End If

                itm.Qty = Convert.ToDecimal(EInvBillDetGrid.Rows(nItemCtr).Cells("itm_Qty").Value)
                itm.Unit = EInvBillDetGrid.Rows(nItemCtr).Cells("itm_Unit").Value.Trim()
                itm.UnitPrice = Convert.ToDecimal(EInvBillDetGrid.Rows(nItemCtr).Cells("itm_UnitPrice").Value)
                itm.TotAmt = Convert.ToDecimal(EInvBillDetGrid.Rows(nItemCtr).Cells("itm_TotAmt").Value)
                itm.Discount = Convert.ToDecimal(EInvBillDetGrid.Rows(nItemCtr).Cells("itm_Discount").Value)
                If nDebug = 0 Then
                    MsgBox("22")
                End If

                itm.AssAmt = Convert.ToDecimal(EInvBillDetGrid.Rows(nItemCtr).Cells("itm_AssAmt").Value)
                itm.GstRt = Convert.ToDecimal(EInvBillDetGrid.Rows(nItemCtr).Cells("itm_GstRt").Value)
                itm.SgstAmt = Convert.ToDecimal(EInvBillDetGrid.Rows(nItemCtr).Cells("itm_SgstAmt").Value)
                itm.IgstAmt = Convert.ToDecimal(EInvBillDetGrid.Rows(nItemCtr).Cells("itm_IgstAmt").Value)
                itm.CgstAmt = Convert.ToDecimal(EInvBillDetGrid.Rows(nItemCtr).Cells("itm_CgstAmt").Value)
                itm.CesRt = Convert.ToDecimal(EInvBillDetGrid.Rows(nItemCtr).Cells("itm_CesRt").Value)
                itm.CesAmt = Convert.ToDecimal(EInvBillDetGrid.Rows(nItemCtr).Cells("itm_CesAmt").Value)

                If nDebug = 0 Then
                    MsgBox("23")
                End If

                itm.CesNonAdvlAmt = Convert.ToDecimal(EInvBillDetGrid.Rows(nItemCtr).Cells("itm_CesNonAdvlAmt").Value)
                itm.StateCesRt = Convert.ToDecimal(EInvBillDetGrid.Rows(nItemCtr).Cells("itm_StateCesRt").Value)
                itm.StateCesAmt = Convert.ToDecimal(EInvBillDetGrid.Rows(nItemCtr).Cells("itm_StateCesAmt").Value)
                itm.StateCesNonAdvlAmt = Convert.ToDecimal(EInvBillDetGrid.Rows(nItemCtr).Cells("itm_StateCesNonAdvlAmt").Value)
                itm.OthChrg = Convert.ToDecimal(EInvBillDetGrid.Rows(nItemCtr).Cells("itm_OthChrg").Value)
                If nDebug = 0 Then
                    MsgBox("24")
                End If

                itm.TotItemVal = Convert.ToDecimal(EInvBillDetGrid.Rows(nItemCtr).Cells("itm_TotItemVal").Value)
                itm.AttribDtls = Nothing

                If nDebug = 0 Then
                    MsgBox(JsonConvert.SerializeObject(itm))
                End If

                eInvGen.ItemList.Add(itm)

                If nDebug = 0 Then
                    MsgBox(JsonConvert.SerializeObject(eInvGen))
                End If

                If nDebug = 0 Then
                    MsgBox("25")
                End If

            Else
                Exit For
            End If
        Next
        nRowIndex = nItemCtr - 1

        If nDebug = 0 Then
            MsgBox("Loop JSON " + JsonConvert.SerializeObject(eInvGen))
        End If

        WriteToErrorLog("Before CallEInvGenAPI for Doc No.: " + cBillNo + " RowIndex : " + nRowIndex)
        Await CallEInvGenAPI(eInvSession, eInvGen, nRowIndex)
        WriteToErrorLog("After CallEInvGenAPI for Doc No.: " + cBillNo + " RowIndex : " + nRowIndex)
        'Await Task.Delay(5000)
    End If
Next
