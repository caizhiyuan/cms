﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SSCMS.Configuration;
using SSCMS.Wx;

namespace SSCMS.Web.Controllers.Admin.Wx
{
    public partial class UsersController
    {
        [HttpPost, Route(RouteActionsAddTag)]
        public async Task<ActionResult<AddTagResult>> AddTag([FromBody] AddTagRequest request)
        {
            if (!await _authManager.HasSitePermissionsAsync(request.SiteId, Types.SitePermissions.WxUsers))
            {
                return Unauthorized();
            }

            List<WxUserTag> tags = null;

            var (success, token, _) = await _wxManager.GetAccessTokenAsync(request.SiteId);
            if (success)
            {
                await _wxManager.AddUserTag(token, request.TagName);
                tags = await _wxManager.GetUserTagsAsync(token);
            }

            return new AddTagResult
            {
                Tags = tags
            };
        }
    }
}
