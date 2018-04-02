
var common = {
    init: function () {
        common.registerEvent();
    },
    registerEvent: function () {
        $("#txtSearch").autocomplete({
            minLength: 0,
            source: function (request, response) {
                $.ajax({
                    url: "/Header/ListName",
                    dataType: "json",
                    data: {
                        q: request.term
                    },
                    success: function (res) {
                        response(res.data);
                    }
                });
            },
            focus: function (event, ui) {
                $("#txtSearch").val(ui.item.label);
                return false;
            },
            select: function (event, ui) {
                $("#txtSearch").val(ui.item.label);
                return false;
            }
        })
     .autocomplete("instance")._renderItem = function (ul, item) {
         return $("<li>")
           .append("<a>" + item.label + "</a>")
           .appendTo(ul);
     };



        function split(val) {
            return val.split(/,\s*/);
        }
        function extractLast(term) {
            return split(term).pop();
        }

        $(".KeywordProduct")
          // don't navigate away from the field on tab when selecting an item
          .bind("keydown", function (event) {
              if (event.keyCode === $.ui.keyCode.TAB &&
                  $(this).autocomplete("instance").menu.active) {
                  event.preventDefault();
              }
          })
          .autocomplete({
              minLength: 0,
              source: function (request, response) {
                  $.ajax({
                      url: "/Productad/ListTag",
                      dataType: "json",
                      data: {
                          q: extractLast(request.term)
                      },
                      success: function (res) {
                          response(res.data);
                      }
                  });
              },
              focus: function () {
                  // prevent value inserted on focus
                  return false;
              },
              select: function (event, ui) {
                  var terms = split(this.value);
                  // remove the current input
                  terms.pop();
                  // add the selected item
                  terms.push(ui.item.value);
                  // add placeholder to get the comma-and-space at the end
                  terms.push("");
                  this.value = terms.join(",");
                  return false;
              }
          });
        //load tag tự động
        $(".KeywordNews")
          // don't navigate away from the field on tab when selecting an item
          .bind("keydown", function (event) {
              if (event.keyCode === $.ui.keyCode.TAB &&
                  $(this).autocomplete("instance").menu.active) {
                  event.preventDefault();
              }
          })
          .autocomplete({
              minLength: 0,
              source: function (request, response) {
                  $.ajax({
                      url: "/Newsad/ListTag",
                      dataType: "json",
                      data: {
                          q: extractLast(request.term)
                      },
                      success: function (res) {
                          response(res.data);
                      }
                  });
              },
              focus: function () {
                  // prevent value inserted on focus
                  return false;
              },
              select: function (event, ui) {
                  var terms = split(this.value);
                  // remove the current input
                  terms.pop();
                  // add the selected item
                  terms.push(ui.item.value);
                  // add placeholder to get the comma-and-space at the end
                  terms.push("");
                  this.value = terms.join(",");
                  return false;
              }
          });
    }
}
common.init();